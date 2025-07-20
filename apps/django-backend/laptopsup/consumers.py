# api/consumers.py
import json
import logging
import asyncio
from channels.generic.websocket import AsyncWebsocketConsumer
from api.grpc_client import grpc_client
import grpc

logger = logging.getLogger(__name__)

class SupportChatConsumer(AsyncWebsocketConsumer):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.current_task: asyncio.Task | None = None

    async def connect(self):
        await self.accept()
        logger.info(f"WebSocket connected: {self.channel_name}")
        await self.send(text_data=json.dumps({
            'type': 'connection_established',
            'channel_name': self.channel_name
        }))

    async def disconnect(self, close_code):
        logger.info(f"WebSocket disconnected: {self.channel_name} (Code: {close_code})")
        # Ensure any running task is cancelled on disconnect
        if self.current_task and not self.current_task.done():
            self.current_task.cancel()

    async def receive(self, text_data):
        """Handles incoming messages from the client, specifically for cancellation."""
        data = json.loads(text_data)
        if data.get('type') == 'cancel_operation':
            logger.info(f"Received cancellation request for channel: {self.channel_name}")
            if self.current_task and not self.current_task.done():
                self.current_task.cancel()
                await self.send(text_data=json.dumps({'type': 'cancellation_status', 'success': True}))
            else:
                logger.warning("No active task to cancel.")
                await self.send(text_data=json.dumps({'type': 'cancellation_status', 'success': False}))

    async def trigger_task(self, event):
        """Receives an internal message from the view to start a long-running task."""
        task_type = event['task_type']
        payload = event['payload']
        logger.info(f"Consumer received task trigger: {task_type} with payload: {payload}")

        if self.current_task and not self.current_task.done():
            logger.warning("Refusing to start new task; one is already in progress.")
            return

        # Create a new coroutine to handle the streaming and forwarding of progress
        self.current_task = asyncio.create_task(self._stream_and_forward(task_type, payload))

    async def _stream_and_forward(self, task_type: str, payload: any):
        """
        Handles the full lifecycle of a gRPC streaming call:
        1. Awaits the async gRPC stub.
        2. Iterates through the stream.
        3. Forwards progress to the client.
        4. Handles errors and cancellation.
        """
        stream_generator = None
        try:
            if task_type == 'install_apps':
                stream_generator = grpc_client.install_apps(payload)
            elif task_type == 'install_environment':
                stream_generator = grpc_client.install_environment(payload)
            
            if stream_generator:
                async for progress in stream_generator:
                    await self.send_progress({"data": progress})

        except asyncio.CancelledError:
            logger.info(f"Task for {self.channel_name} was cancelled.")
            await self.send_progress({
                "data": {"message": "Operation cancelled by user.", "status": "FAILED"}
            })
        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC error for {self.channel_name}: {e.details()}")
            await self.send_progress({
                "data": {"error": {"code": str(e.code()), "message": e.details()}, "status": "FAILED"}
            })
        finally:
            self.current_task = None # Clear the task reference when done

    async def send_progress(self, event):
        """Handler for progress update events sent from the background tasks."""
        await self.send(text_data=json.dumps({
            'type': 'progress_update',
            'payload': event["data"]
        }))
