// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: authorization.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace tobeh.Valmar {
  /// <summary>
  /// Service definition for authorization
  /// </summary>
  public static partial class Authorization
  {
    static readonly string __ServiceName = "authorization.Authorization";

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
    static readonly grpc::Marshaller<global::tobeh.Valmar.GetAvailableScopesMessage> __Marshaller_authorization_GetAvailableScopesMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.GetAvailableScopesMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.ScopeMessage> __Marshaller_authorization_ScopeMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.ScopeMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage> __Marshaller_authorization_CreateOAuth2AuthorizationCodeMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.OAuth2AuthorizationCodeMessage> __Marshaller_authorization_OAuth2AuthorizationCodeMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.OAuth2AuthorizationCodeMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage> __Marshaller_authorization_OAuth2AuthorizationCodeExchangeMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.OAuth2AccessTokenMessage> __Marshaller_authorization_OAuth2AccessTokenMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.OAuth2AccessTokenMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.CreateOAuth2TokenMessage> __Marshaller_authorization_CreateOAuth2TokenMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.CreateOAuth2TokenMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.CreateOAuth2ClientMessage> __Marshaller_authorization_CreateOAuth2ClientMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.CreateOAuth2ClientMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.OAuth2ClientMessage> __Marshaller_authorization_OAuth2ClientMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.OAuth2ClientMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.GetAvailableScopesMessage, global::tobeh.Valmar.ScopeMessage> __Method_GetAvailableScopes = new grpc::Method<global::tobeh.Valmar.GetAvailableScopesMessage, global::tobeh.Valmar.ScopeMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetAvailableScopes",
        __Marshaller_authorization_GetAvailableScopesMessage,
        __Marshaller_authorization_ScopeMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage, global::tobeh.Valmar.OAuth2AuthorizationCodeMessage> __Method_CreateOAuth2AuthorizationCode = new grpc::Method<global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage, global::tobeh.Valmar.OAuth2AuthorizationCodeMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateOAuth2AuthorizationCode",
        __Marshaller_authorization_CreateOAuth2AuthorizationCodeMessage,
        __Marshaller_authorization_OAuth2AuthorizationCodeMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage, global::tobeh.Valmar.OAuth2AccessTokenMessage> __Method_ExchangeOauth2AuthorizationCode = new grpc::Method<global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage, global::tobeh.Valmar.OAuth2AccessTokenMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ExchangeOauth2AuthorizationCode",
        __Marshaller_authorization_OAuth2AuthorizationCodeExchangeMessage,
        __Marshaller_authorization_OAuth2AccessTokenMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.CreateOAuth2TokenMessage, global::tobeh.Valmar.OAuth2AccessTokenMessage> __Method_CreateOauth2Token = new grpc::Method<global::tobeh.Valmar.CreateOAuth2TokenMessage, global::tobeh.Valmar.OAuth2AccessTokenMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateOauth2Token",
        __Marshaller_authorization_CreateOAuth2TokenMessage,
        __Marshaller_authorization_OAuth2AccessTokenMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.CreateOAuth2ClientMessage, global::tobeh.Valmar.OAuth2ClientMessage> __Method_CreateOauth2Client = new grpc::Method<global::tobeh.Valmar.CreateOAuth2ClientMessage, global::tobeh.Valmar.OAuth2ClientMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateOauth2Client",
        __Marshaller_authorization_CreateOAuth2ClientMessage,
        __Marshaller_authorization_OAuth2ClientMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.OAuth2ClientMessage> __Method_GetOauth2Clients = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.OAuth2ClientMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetOauth2Clients",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_authorization_OAuth2ClientMessage);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::tobeh.Valmar.AuthorizationReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for Authorization</summary>
    public partial class AuthorizationClient : grpc::ClientBase<AuthorizationClient>
    {
      /// <summary>Creates a new client for Authorization</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AuthorizationClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Authorization that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public AuthorizationClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AuthorizationClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected AuthorizationClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Gets all available scopes
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.ScopeMessage> GetAvailableScopes(global::tobeh.Valmar.GetAvailableScopesMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAvailableScopes(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets all available scopes
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.ScopeMessage> GetAvailableScopes(global::tobeh.Valmar.GetAvailableScopesMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetAvailableScopes, null, options, request);
      }
      /// <summary>
      /// Creates a new OAuth2 authorization code for a given client and typo member
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AuthorizationCodeMessage CreateOAuth2AuthorizationCode(global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOAuth2AuthorizationCode(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Creates a new OAuth2 authorization code for a given client and typo member
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AuthorizationCodeMessage CreateOAuth2AuthorizationCode(global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateOAuth2AuthorizationCode, null, options, request);
      }
      /// <summary>
      /// Creates a new OAuth2 authorization code for a given client and typo member
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AuthorizationCodeMessage> CreateOAuth2AuthorizationCodeAsync(global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOAuth2AuthorizationCodeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Creates a new OAuth2 authorization code for a given client and typo member
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AuthorizationCodeMessage> CreateOAuth2AuthorizationCodeAsync(global::tobeh.Valmar.CreateOAuth2AuthorizationCodeMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateOAuth2AuthorizationCode, null, options, request);
      }
      /// <summary>
      /// Exchange a oauth2 authorization code for an access token (jwt)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AccessTokenMessage ExchangeOauth2AuthorizationCode(global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ExchangeOauth2AuthorizationCode(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Exchange a oauth2 authorization code for an access token (jwt)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AccessTokenMessage ExchangeOauth2AuthorizationCode(global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ExchangeOauth2AuthorizationCode, null, options, request);
      }
      /// <summary>
      /// Exchange a oauth2 authorization code for an access token (jwt)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AccessTokenMessage> ExchangeOauth2AuthorizationCodeAsync(global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ExchangeOauth2AuthorizationCodeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Exchange a oauth2 authorization code for an access token (jwt)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AccessTokenMessage> ExchangeOauth2AuthorizationCodeAsync(global::tobeh.Valmar.OAuth2AuthorizationCodeExchangeMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ExchangeOauth2AuthorizationCode, null, options, request);
      }
      /// <summary>
      /// Create a oauth2 access token. should be used only in authorized preconditions like auth code or token exchange grants
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AccessTokenMessage CreateOauth2Token(global::tobeh.Valmar.CreateOAuth2TokenMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOauth2Token(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Create a oauth2 access token. should be used only in authorized preconditions like auth code or token exchange grants
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2AccessTokenMessage CreateOauth2Token(global::tobeh.Valmar.CreateOAuth2TokenMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateOauth2Token, null, options, request);
      }
      /// <summary>
      /// Create a oauth2 access token. should be used only in authorized preconditions like auth code or token exchange grants
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AccessTokenMessage> CreateOauth2TokenAsync(global::tobeh.Valmar.CreateOAuth2TokenMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOauth2TokenAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Create a oauth2 access token. should be used only in authorized preconditions like auth code or token exchange grants
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2AccessTokenMessage> CreateOauth2TokenAsync(global::tobeh.Valmar.CreateOAuth2TokenMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateOauth2Token, null, options, request);
      }
      /// <summary>
      /// Creates a new OAuth2 client
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2ClientMessage CreateOauth2Client(global::tobeh.Valmar.CreateOAuth2ClientMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOauth2Client(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Creates a new OAuth2 client
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.OAuth2ClientMessage CreateOauth2Client(global::tobeh.Valmar.CreateOAuth2ClientMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateOauth2Client, null, options, request);
      }
      /// <summary>
      /// Creates a new OAuth2 client
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2ClientMessage> CreateOauth2ClientAsync(global::tobeh.Valmar.CreateOAuth2ClientMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateOauth2ClientAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Creates a new OAuth2 client
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.OAuth2ClientMessage> CreateOauth2ClientAsync(global::tobeh.Valmar.CreateOAuth2ClientMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateOauth2Client, null, options, request);
      }
      /// <summary>
      /// Gets all OAuth2 clients
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.OAuth2ClientMessage> GetOauth2Clients(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetOauth2Clients(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets all OAuth2 clients
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::tobeh.Valmar.OAuth2ClientMessage> GetOauth2Clients(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetOauth2Clients, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override AuthorizationClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AuthorizationClient(configuration);
      }
    }

  }
}
#endregion
