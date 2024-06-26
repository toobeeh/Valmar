// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: leagues.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace tobeh.Valmar {
  /// <summary>
  /// Service definition for league stat access
  /// </summary>
  public static partial class Leagues
  {
    static readonly string __ServiceName = "leagues.Leagues";

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
    static readonly grpc::Marshaller<global::tobeh.Valmar.LeagueSeasonEvaluationReply> __Marshaller_leagues_LeagueSeasonEvaluationReply = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.LeagueSeasonEvaluationReply.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.EvaluateSeasonRequest> __Marshaller_leagues_EvaluateSeasonRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.EvaluateSeasonRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest> __Marshaller_leagues_EvaluateMemberCurrentSeasonRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> __Marshaller_leagues_LeagueSeasonMemberEvaluationReply = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::tobeh.Valmar.EvaluateMemberSeasonRequest> __Marshaller_leagues_EvaluateMemberSeasonRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::tobeh.Valmar.EvaluateMemberSeasonRequest.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.LeagueSeasonEvaluationReply> __Method_EvaluateCurrentLeagueSeason = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::tobeh.Valmar.LeagueSeasonEvaluationReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EvaluateCurrentLeagueSeason",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_leagues_LeagueSeasonEvaluationReply);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.EvaluateSeasonRequest, global::tobeh.Valmar.LeagueSeasonEvaluationReply> __Method_EvaluateLeagueSeason = new grpc::Method<global::tobeh.Valmar.EvaluateSeasonRequest, global::tobeh.Valmar.LeagueSeasonEvaluationReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EvaluateLeagueSeason",
        __Marshaller_leagues_EvaluateSeasonRequest,
        __Marshaller_leagues_LeagueSeasonEvaluationReply);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest, global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> __Method_EvaluateMemberCurrentLeagueSeason = new grpc::Method<global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest, global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EvaluateMemberCurrentLeagueSeason",
        __Marshaller_leagues_EvaluateMemberCurrentSeasonRequest,
        __Marshaller_leagues_LeagueSeasonMemberEvaluationReply);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::tobeh.Valmar.EvaluateMemberSeasonRequest, global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> __Method_EvaluateMemberLeagueSeason = new grpc::Method<global::tobeh.Valmar.EvaluateMemberSeasonRequest, global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EvaluateMemberLeagueSeason",
        __Marshaller_leagues_EvaluateMemberSeasonRequest,
        __Marshaller_leagues_LeagueSeasonMemberEvaluationReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::tobeh.Valmar.LeaguesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for Leagues</summary>
    public partial class LeaguesClient : grpc::ClientBase<LeaguesClient>
    {
      /// <summary>Creates a new client for Leagues</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public LeaguesClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Leagues that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public LeaguesClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected LeaguesClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected LeaguesClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Gets the current league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonEvaluationReply EvaluateCurrentLeagueSeason(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateCurrentLeagueSeason(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets the current league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonEvaluationReply EvaluateCurrentLeagueSeason(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EvaluateCurrentLeagueSeason, null, options, request);
      }
      /// <summary>
      /// Gets the current league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonEvaluationReply> EvaluateCurrentLeagueSeasonAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateCurrentLeagueSeasonAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets the current league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonEvaluationReply> EvaluateCurrentLeagueSeasonAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EvaluateCurrentLeagueSeason, null, options, request);
      }
      /// <summary>
      /// gets the league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonEvaluationReply EvaluateLeagueSeason(global::tobeh.Valmar.EvaluateSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateLeagueSeason(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// gets the league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonEvaluationReply EvaluateLeagueSeason(global::tobeh.Valmar.EvaluateSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EvaluateLeagueSeason, null, options, request);
      }
      /// <summary>
      /// gets the league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonEvaluationReply> EvaluateLeagueSeasonAsync(global::tobeh.Valmar.EvaluateSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateLeagueSeasonAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// gets the league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonEvaluationReply> EvaluateLeagueSeasonAsync(global::tobeh.Valmar.EvaluateSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EvaluateLeagueSeason, null, options, request);
      }
      /// <summary>
      /// Gets the current own league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply EvaluateMemberCurrentLeagueSeason(global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateMemberCurrentLeagueSeason(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets the current own league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply EvaluateMemberCurrentLeagueSeason(global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EvaluateMemberCurrentLeagueSeason, null, options, request);
      }
      /// <summary>
      /// Gets the current own league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> EvaluateMemberCurrentLeagueSeasonAsync(global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateMemberCurrentLeagueSeasonAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Gets the current own league evaluation
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> EvaluateMemberCurrentLeagueSeasonAsync(global::tobeh.Valmar.EvaluateMemberCurrentSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EvaluateMemberCurrentLeagueSeason, null, options, request);
      }
      /// <summary>
      /// gets the own league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply EvaluateMemberLeagueSeason(global::tobeh.Valmar.EvaluateMemberSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateMemberLeagueSeason(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// gets the own league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply EvaluateMemberLeagueSeason(global::tobeh.Valmar.EvaluateMemberSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EvaluateMemberLeagueSeason, null, options, request);
      }
      /// <summary>
      /// gets the own league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> EvaluateMemberLeagueSeasonAsync(global::tobeh.Valmar.EvaluateMemberSeasonRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EvaluateMemberLeagueSeasonAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// gets the own league evaluation for a specific month
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::tobeh.Valmar.LeagueSeasonMemberEvaluationReply> EvaluateMemberLeagueSeasonAsync(global::tobeh.Valmar.EvaluateMemberSeasonRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EvaluateMemberLeagueSeason, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override LeaguesClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new LeaguesClient(configuration);
      }
    }

  }
}
#endregion
