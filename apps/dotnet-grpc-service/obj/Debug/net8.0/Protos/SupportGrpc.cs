// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/support.proto
// </auto-generated>
// Original file comments:
// Protos/support.proto
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace LaptopSupport {
  public static partial class SupportService
  {
    static readonly string __ServiceName = "LaptopSupport.SupportService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.InstallAppsRequest> __Marshaller_LaptopSupport_InstallAppsRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.InstallAppsRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.ProgressUpdate> __Marshaller_LaptopSupport_ProgressUpdate = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.ProgressUpdate.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.QuerySystemInfoRequest> __Marshaller_LaptopSupport_QuerySystemInfoRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.QuerySystemInfoRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.SystemInfoResponse> __Marshaller_LaptopSupport_SystemInfoResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.SystemInfoResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.InstallEnvironmentRequest> __Marshaller_LaptopSupport_InstallEnvironmentRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.InstallEnvironmentRequest.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::LaptopSupport.InstallAppsRequest, global::LaptopSupport.ProgressUpdate> __Method_InstallApps = new grpc::Method<global::LaptopSupport.InstallAppsRequest, global::LaptopSupport.ProgressUpdate>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "InstallApps",
        __Marshaller_LaptopSupport_InstallAppsRequest,
        __Marshaller_LaptopSupport_ProgressUpdate);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::LaptopSupport.QuerySystemInfoRequest, global::LaptopSupport.SystemInfoResponse> __Method_QuerySystemInfo = new grpc::Method<global::LaptopSupport.QuerySystemInfoRequest, global::LaptopSupport.SystemInfoResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "QuerySystemInfo",
        __Marshaller_LaptopSupport_QuerySystemInfoRequest,
        __Marshaller_LaptopSupport_SystemInfoResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::LaptopSupport.InstallEnvironmentRequest, global::LaptopSupport.ProgressUpdate> __Method_InstallEnvironment = new grpc::Method<global::LaptopSupport.InstallEnvironmentRequest, global::LaptopSupport.ProgressUpdate>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "InstallEnvironment",
        __Marshaller_LaptopSupport_InstallEnvironmentRequest,
        __Marshaller_LaptopSupport_ProgressUpdate);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::LaptopSupport.SupportReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of SupportService</summary>
    [grpc::BindServiceMethod(typeof(SupportService), "BindService")]
    public abstract partial class SupportServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task InstallApps(global::LaptopSupport.InstallAppsRequest request, grpc::IServerStreamWriter<global::LaptopSupport.ProgressUpdate> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::LaptopSupport.SystemInfoResponse> QuerySystemInfo(global::LaptopSupport.QuerySystemInfoRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task InstallEnvironment(global::LaptopSupport.InstallEnvironmentRequest request, grpc::IServerStreamWriter<global::LaptopSupport.ProgressUpdate> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(SupportServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_InstallApps, serviceImpl.InstallApps)
          .AddMethod(__Method_QuerySystemInfo, serviceImpl.QuerySystemInfo)
          .AddMethod(__Method_InstallEnvironment, serviceImpl.InstallEnvironment).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, SupportServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_InstallApps, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::LaptopSupport.InstallAppsRequest, global::LaptopSupport.ProgressUpdate>(serviceImpl.InstallApps));
      serviceBinder.AddMethod(__Method_QuerySystemInfo, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::LaptopSupport.QuerySystemInfoRequest, global::LaptopSupport.SystemInfoResponse>(serviceImpl.QuerySystemInfo));
      serviceBinder.AddMethod(__Method_InstallEnvironment, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::LaptopSupport.InstallEnvironmentRequest, global::LaptopSupport.ProgressUpdate>(serviceImpl.InstallEnvironment));
    }

  }
  public static partial class AdminService
  {
    static readonly string __ServiceName = "LaptopSupport.AdminService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.RunCommandRequest> __Marshaller_LaptopSupport_RunCommandRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.RunCommandRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::LaptopSupport.RunCommandResponse> __Marshaller_LaptopSupport_RunCommandResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::LaptopSupport.RunCommandResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::LaptopSupport.RunCommandRequest, global::LaptopSupport.RunCommandResponse> __Method_RunCommand = new grpc::Method<global::LaptopSupport.RunCommandRequest, global::LaptopSupport.RunCommandResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RunCommand",
        __Marshaller_LaptopSupport_RunCommandRequest,
        __Marshaller_LaptopSupport_RunCommandResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::LaptopSupport.SupportReflection.Descriptor.Services[1]; }
    }

    /// <summary>Base class for server-side implementations of AdminService</summary>
    [grpc::BindServiceMethod(typeof(AdminService), "BindService")]
    public abstract partial class AdminServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::LaptopSupport.RunCommandResponse> RunCommand(global::LaptopSupport.RunCommandRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(AdminServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_RunCommand, serviceImpl.RunCommand).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, AdminServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_RunCommand, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::LaptopSupport.RunCommandRequest, global::LaptopSupport.RunCommandResponse>(serviceImpl.RunCommand));
    }

  }
}
#endregion
