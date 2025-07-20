# api/views.py
import asyncio
import logging
from datetime import timedelta
from django.utils import timezone
from django.db.models import Avg, Count
from django.db.models.functions import TruncHour
import grpc 
from asgiref.sync import async_to_sync # Import the correct adapter

from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from rest_framework.permissions import IsAuthenticated, IsAdminUser, AllowAny

from channels.layers import get_channel_layer
from .grpc_client import grpc_client
from .models import SystemMetric, OperationLog

from .services import ai_classifier_instance as ai_classifier
from .services import response_formatter_instance as response_formatter

logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)

class AiRequestView(APIView):
    permission_classes = [AllowAny]

    @async_to_sync
    async def post(self, request, *args, **kwargs):
        """
        FIX: The view is now wrapped with async_to_sync. This allows the async code
        inside to run correctly on the main event loop managed by Daphne,
        which solves the 'received a <class 'coroutine'>' error.
        """
        logger.debug(f"Received POST request for /api/ai-request/ with data: {request.data}")
        try:
            query = request.data.get("query")
            channel_name = request.data.get("channel_name")
            logger.debug(f"Handling async request for query: '{query}' on channel: {channel_name}")

            if not query or not channel_name:
                raise ValueError("Query and channel_name are required.")

            classification_result = await ai_classifier.classify(query)
            logger.debug(f"AI Classification result: {classification_result}")
            intent = classification_result.get("intent")
            entities = classification_result.get("entities", {})
            chat_response = response_formatter.generate_response(classification_result)

            channel_layer = get_channel_layer()
            task_started = False

            if intent == "app_installation" and entities.get("apps"):
                logger.debug(f"Sending trigger to consumer for app installation: {entities['apps']}")
                await channel_layer.send(channel_name, {
                    "type": "trigger.task",
                    "task_type": "install_apps",
                    "payload": entities["apps"]
                })
                task_started = True
            elif intent == "environment_setup" and entities.get("environment"):
                logger.debug(f"Sending trigger to consumer for env setup: {entities['environment']}")
                await channel_layer.send(channel_name, {
                    "type": "trigger.task",
                    "task_type": "install_environment",
                    "payload": entities["environment"]
                })
                task_started = True
            elif intent == "hardware_info":
                logger.debug("Querying for hardware info via gRPC.")
                system_info = await grpc_client.query_system_info(False, True, True)
                chat_response += f"\n\n**System Specs:**\n```json\n{system_info}\n```"
            
            # if not task_started:
            logger.debug("No long-running task started. Sending completion signal immediately.")
            await self.send_completion_signal(channel_name)

            response_data = {"initial_response": chat_response, "intent": intent}
            logger.debug(f"Returning successful response: {response_data}")
            return Response(response_data, status=status.HTTP_200_OK)

        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC connection error in AiRequestView: {e.details()}", exc_info=True)
            error_message = {
                "error": "Cannot connect to the backend support service.",
                "details": "Please ensure the C# gRPC service is running and accessible.",
                "grpc_code": str(e.code())
            }
            return Response(error_message, status=status.HTTP_503_SERVICE_UNAVAILABLE)
        except Exception as e:
            logger.error(f"Error in AiRequestView: {e}", exc_info=True)
            return Response({"error": "An internal server error occurred."},
                            status=status.HTTP_500_INTERNAL_SERVER_ERROR)

    async def send_completion_signal(self, channel_name: str):
        """Sends a final completion message to the client."""
        logger.debug(f"Sending completion signal to channel: {channel_name}")
        channel_layer = get_channel_layer()
        await channel_layer.send(channel_name, {
            "type": "send.progress",
            "data": {
                "message": "Task complete.",
                "percentage_complete": 100,
                "status": "COMPLETED"
            }
        })


class AdminOperationView(APIView):
    permission_classes = [IsAdminUser]

    @async_to_sync
    async def post(self, request, *args, **kwargs):
        command = request.data.get("command")
        args = request.data.get("args", [])
        if not command:
            return Response({"error": "Command is required."}, status=status.HTTP_400_BAD_REQUEST)

        logger.warning(f"Admin user '{request.user}' is executing command: {command}")
        result = await grpc_client.run_admin_command(command, args)
        return Response(result, status=status.HTTP_200_OK)


class AnalyticsDashboardView(APIView):
    permission_classes = [IsAdminUser]

    def get(self, request, *args, **kwargs):
        time_window = timezone.now() - timedelta(hours=24)
        cpu_usage = SystemMetric.objects.filter(metric_name='cpu_usage', timestamp__gte=time_window).annotate(
            hour=TruncHour('timestamp')).values('hour').annotate(avg_value=Avg('metric_value')).order_by('hour')
        api_requests = OperationLog.objects.filter(timestamp__gte=time_window).annotate(
            hour=TruncHour('timestamp')).values('hour').annotate(count=Count('id')).order_by('hour')
        operation_status = OperationLog.objects.filter(timestamp__gte=time_window).values('is_success').annotate(
            count=Count('is_success'))

        return Response({
            "time_series": {"cpu_usage_hourly": list(cpu_usage), "api_requests_hourly": list(api_requests)},
            "summary": {"total_requests_24h": OperationLog.objects.filter(timestamp__gte=time_window).count(),
                        "operation_status": list(operation_status)}
        }, status=status.HTTP_200_OK)
