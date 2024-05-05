// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: card.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace tobeh.Valmar {
  /// <summary>
  /// Service definition for card resource access
  /// </summary>
  public static partial class Card
  {
    static readonly string __ServiceName = "card.Card";

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
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.CardTemplateListingMessage> __Marshaller_card_CardTemplateListingMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.CardTemplateListingMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.GetCardTemplateMessage> __Marshaller_card_GetCardTemplateMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.GetCardTemplateMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.CardTemplateMessage> __Marshaller_card_CardTemplateMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.CardTemplateMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.GetMemberCardSettingsMessage> __Marshaller_card_GetMemberCardSettingsMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.GetMemberCardSettingsMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.MemberCardSettingsMessage> __Marshaller_card_MemberCardSettingsMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.MemberCardSettingsMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.SetMemberCardSettingsMessage> __Marshaller_card_SetMemberCardSettingsMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.SetMemberCardSettingsMessage.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.CardTemplateListingMessage> __Method_GetCardTemplates = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.CardTemplateListingMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetCardTemplates",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_card_CardTemplateListingMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.GetCardTemplateMessage, global::tobeh.Valmar.CardTemplateMessage> __Method_GetCardTemplate = new grpc::Method<global::tobeh.Valmar.GetCardTemplateMessage, global::tobeh.Valmar.CardTemplateMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetCardTemplate",
        __Marshaller_card_GetCardTemplateMessage,
        __Marshaller_card_CardTemplateMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.GetMemberCardSettingsMessage, global::tobeh.Valmar.MemberCardSettingsMessage> __Method_GetMemberCardSettings = new grpc::Method<global::tobeh.Valmar.GetMemberCardSettingsMessage, global::tobeh.Valmar.MemberCardSettingsMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetMemberCardSettings",
        __Marshaller_card_GetMemberCardSettingsMessage,
        __Marshaller_card_MemberCardSettingsMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.SetMemberCardSettingsMessage, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMemberCardSettings = new grpc::Method<global::tobeh.Valmar.SetMemberCardSettingsMessage, global::Google.Protobuf.WellKnownTypes.Empty>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SetMemberCardSettings",
        __Marshaller_card_SetMemberCardSettingsMessage,
        __Marshaller_google_protobuf_Empty);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::tobeh.Valmar.CardReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for Card</summary>
    public partial class CardClient : grpc::ClientBase<CardClient>
    {
      /// <summary>Creates a new client for Card</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CardClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Card that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CardClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CardClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CardClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// get all available card templates
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.CardTemplateListingMessage> GetCardTemplates(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCardTemplates(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// get all available card templates
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.CardTemplateListingMessage> GetCardTemplates(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetCardTemplates, null, options, request);
      }
      /// <summary>
      /// get a single card template
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.CardTemplateMessage GetCardTemplate(global::tobeh.Valmar.GetCardTemplateMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCardTemplate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// get a single card template
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.CardTemplateMessage GetCardTemplate(global::tobeh.Valmar.GetCardTemplateMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetCardTemplate, null, options, request);
      }
      /// <summary>
      /// get a single card template
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.CardTemplateMessage> GetCardTemplateAsync(global::tobeh.Valmar.GetCardTemplateMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCardTemplateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// get a single card template
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.CardTemplateMessage> GetCardTemplateAsync(global::tobeh.Valmar.GetCardTemplateMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetCardTemplate, null, options, request);
      }
      /// <summary>
      /// get member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.MemberCardSettingsMessage GetMemberCardSettings(global::tobeh.Valmar.GetMemberCardSettingsMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMemberCardSettings(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// get member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.MemberCardSettingsMessage GetMemberCardSettings(global::tobeh.Valmar.GetMemberCardSettingsMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetMemberCardSettings, null, options, request);
      }
      /// <summary>
      /// get member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.MemberCardSettingsMessage> GetMemberCardSettingsAsync(global::tobeh.Valmar.GetMemberCardSettingsMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMemberCardSettingsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// get member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.MemberCardSettingsMessage> GetMemberCardSettingsAsync(global::tobeh.Valmar.GetMemberCardSettingsMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetMemberCardSettings, null, options, request);
      }
      /// <summary>
      /// set member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Google.Protobuf.WellKnownTypes.Empty SetMemberCardSettings(global::tobeh.Valmar.SetMemberCardSettingsMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SetMemberCardSettings(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// set member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Google.Protobuf.WellKnownTypes.Empty SetMemberCardSettings(global::tobeh.Valmar.SetMemberCardSettingsMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SetMemberCardSettings, null, options, request);
      }
      /// <summary>
      /// set member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Google.Protobuf.WellKnownTypes.Empty> SetMemberCardSettingsAsync(global::tobeh.Valmar.SetMemberCardSettingsMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SetMemberCardSettingsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// set member card settings
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Google.Protobuf.WellKnownTypes.Empty> SetMemberCardSettingsAsync(global::tobeh.Valmar.SetMemberCardSettingsMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SetMemberCardSettings, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override CardClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new CardClient(configuration);
      }
    }

  }
}
#endregion