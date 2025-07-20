# api/grpc_client.py
import asyncio
import logging
from typing import Dict, Any, AsyncGenerator

import grpc
from google.protobuf import json_format

# Import generated gRPC files
from . import support_pb2
from . import support_pb2_grpc

# Set logger level to DEBUG to see all log messages
logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)

# --- IMPORTANT CONFIGURATION ---
# This host and port MUST match the address the C# gRPC service is running on.
# Check your C# project's Properties/launchSettings.json file to find the correct port.
GRPC_SERVER_HOST = "localhost"
GRPC_SERVER_PORT = 5215


# --- gRPC Interceptor for Logging ---
class LoggingInterceptor(grpc.aio.UnaryUnaryClientInterceptor, grpc.aio.UnaryStreamClientInterceptor):
    """
    A gRPC interceptor that logs the start and end of RPC calls.
    """

    async def intercept_unary_unary(self, continuation, client_call_details, request):
        logger.info(f"[gRPC] Starting Unary-Unary call to {client_call_details.method}")
        response = await continuation(client_call_details, request)
        logger.info(f"[gRPC] Finished Unary-Unary call to {client_call_details.method}")
        return response

    async def intercept_unary_stream(self, continuation, client_call_details, request):
        logger.info(f"[gRPC] Starting Unary-Stream call to {client_call_details.method}")
        # The continuation returns a response object that can be iterated over.
        response = await continuation(client_call_details, request)
        logger.info(f"[gRPC] Stream initiated for {client_call_details.method}")
        return response


class GrpcClientWrapper:
    """
    A singleton wrapper for the gRPC client to manage the channel
    and provide a simple interface for calling RPC methods.
    """
    _instance = None
    _channel = None  # Initialize channel to None

    def __new__(cls):
        if cls._instance is None:
            cls._instance = super(GrpcClientWrapper, cls).__new__(cls)
            # Do NOT create the channel here.
        return cls._instance

    async def _get_channel(self):
        """
        Lazily create the gRPC channel on first use inside an async context.
        This ensures an event loop is running.
        """
        if self._channel is None:
            addr = f"{GRPC_SERVER_HOST}:{GRPC_SERVER_PORT}"

            # FIX: Pass the interceptor directly during channel creation
            # instead of using the separate intercept_channel function.
            logging_interceptor = LoggingInterceptor()
            self._channel = grpc.aio.insecure_channel(
                addr,
                interceptors=[logging_interceptor]
            )

            logger.info(f"gRPC channel with interceptors created for {addr}")
        return self._channel

    async def get_stub(self):
        """Asynchronously gets the service stub, ensuring the channel is created."""
        channel = await self._get_channel()
        return support_pb2_grpc.SupportServiceStub(channel)

    async def get_admin_stub(self):
        """Asynchronously gets the admin stub, ensuring the channel is created."""
        channel = await self._get_channel()
        return support_pb2_grpc.AdminServiceStub(channel)

    async def install_apps(self, app_ids: list[str]) -> AsyncGenerator[Dict[str, Any], None]:
        """Calls the InstallApps RPC and yields progress updates."""
        stub = await self.get_stub()  # Await the stub
        request = support_pb2.InstallAppsRequest(app_ids=app_ids)
        try:
            stream = stub.InstallApps(request, timeout=300.0)
            async for response in stream:
                yield json_format.MessageToDict(response, preserving_proto_field_name=True)
        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC error during InstallApps: {e.details()}")
            yield {"error": {"code": str(e.code()), "message": e.details()}}

    async def install_environment(self, environment_id: str) -> AsyncGenerator[Dict[str, Any], None]:
        """Calls the InstallEnvironment RPC and yields progress updates."""
        stub = await self.get_stub()  # Await the stub
        request = support_pb2.InstallEnvironmentRequest(environment_id=environment_id)
        try:
            stream = stub.InstallEnvironment(request, timeout=300.0)
            async for response in stream:
                yield json_format.MessageToDict(response, preserving_proto_field_name=True)
        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC error during InstallEnvironment: {e.details()}")
            yield {"error": {"code": str(e.code()), "message": e.details()}}

    async def query_system_info(self, include_drivers: bool, include_gpu: bool, include_storage: bool) -> Dict[
        str, Any]:
        """Calls the QuerySystemInfo RPC and returns system specs."""
        stub = await self.get_stub()  # Await the stub
        request = support_pb2.QuerySystemInfoRequest(
            include_drivers=include_drivers,
            include_gpu_info=include_gpu,
            include_storage_info=include_storage
        )
        try:
            response = await stub.QuerySystemInfo(request, timeout=30.0)
            return json_format.MessageToDict(response, preserving_proto_field_name=True)
        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC error during QuerySystemInfo: {e.details()}")
            return {"error": {"code": str(e.code()), "message": e.details()}}

    async def run_admin_command(self, command: str, args: list[str]):
        stub = await self.get_admin_stub()  # Await the stub
        request = support_pb2.RunCommandRequest(command=command, args=args)
        try:
            response = await stub.RunCommand(request, timeout=30.0)
            return json_format.MessageToDict(response, preserving_proto_field_name=True)
        except grpc.aio.AioRpcError as e:
            logger.error(f"gRPC error during RunCommand: {e.details()}")
            return {"error": {"code": str(e.code()), "message": e.details()}}


# Singleton instance to be used across the Django app
grpc_client = GrpcClientWrapper()
