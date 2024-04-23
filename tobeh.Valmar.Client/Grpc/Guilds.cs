// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: guilds.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace tobeh.Valmar {

  /// <summary>Holder for reflection information generated from guilds.proto</summary>
  public static partial class GuildsReflection {

    #region Descriptor
    /// <summary>File descriptor for guilds.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static GuildsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxndWlsZHMucHJvdG8SBmd1aWxkcyKFAQoKR3VpbGRSZXBseRIPCgdndWls",
            "ZElkGAEgASgDEhEKCWNoYW5uZWxJZBgCIAEoAxIRCgltZXNzYWdlSWQYAyAB",
            "KAMSFAoMb2JzZXJ2ZVRva2VuGAQgASgFEgwKBG5hbWUYBSABKAkSHAoUY29u",
            "bmVjdGVkTWVtYmVyQ291bnQYBiABKAUiJwoPR2V0R3VpbGRSZXF1ZXN0EhQK",
            "DG9ic2VydmVUb2tlbhgBIAEoBSIoChNHZXRHdWlsZEJ5SWRNZXNzYWdlEhEK",
            "CWRpc2NvcmRJZBgBIAEoAzKQAQoGR3VpbGRzEj4KD0dldEd1aWxkQnlUb2tl",
            "bhIXLmd1aWxkcy5HZXRHdWlsZFJlcXVlc3QaEi5ndWlsZHMuR3VpbGRSZXBs",
            "eRJGChNHZXRHdWlsZEJ5RGlzY29yZElkEhsuZ3VpbGRzLkdldEd1aWxkQnlJ",
            "ZE1lc3NhZ2UaEi5ndWlsZHMuR3VpbGRSZXBseUIPqgIMdG9iZWguVmFsbWFy",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.GuildReply), global::tobeh.Valmar.GuildReply.Parser, new[]{ "GuildId", "ChannelId", "MessageId", "ObserveToken", "Name", "ConnectedMemberCount" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.GetGuildRequest), global::tobeh.Valmar.GetGuildRequest.Parser, new[]{ "ObserveToken" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.GetGuildByIdMessage), global::tobeh.Valmar.GetGuildByIdMessage.Parser, new[]{ "DiscordId" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Response containing a guild's properties.
  /// </summary>
  public sealed partial class GuildReply : pb::IMessage<GuildReply>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GuildReply> _parser = new pb::MessageParser<GuildReply>(() => new GuildReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<GuildReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.GuildsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GuildReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GuildReply(GuildReply other) : this() {
      guildId_ = other.guildId_;
      channelId_ = other.channelId_;
      messageId_ = other.messageId_;
      observeToken_ = other.observeToken_;
      name_ = other.name_;
      connectedMemberCount_ = other.connectedMemberCount_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GuildReply Clone() {
      return new GuildReply(this);
    }

    /// <summary>Field number for the "guildId" field.</summary>
    public const int GuildIdFieldNumber = 1;
    private long guildId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long GuildId {
      get { return guildId_; }
      set {
        guildId_ = value;
      }
    }

    /// <summary>Field number for the "channelId" field.</summary>
    public const int ChannelIdFieldNumber = 2;
    private long channelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long ChannelId {
      get { return channelId_; }
      set {
        channelId_ = value;
      }
    }

    /// <summary>Field number for the "messageId" field.</summary>
    public const int MessageIdFieldNumber = 3;
    private long messageId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long MessageId {
      get { return messageId_; }
      set {
        messageId_ = value;
      }
    }

    /// <summary>Field number for the "observeToken" field.</summary>
    public const int ObserveTokenFieldNumber = 4;
    private int observeToken_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ObserveToken {
      get { return observeToken_; }
      set {
        observeToken_ = value;
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 5;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "connectedMemberCount" field.</summary>
    public const int ConnectedMemberCountFieldNumber = 6;
    private int connectedMemberCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ConnectedMemberCount {
      get { return connectedMemberCount_; }
      set {
        connectedMemberCount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as GuildReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(GuildReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GuildId != other.GuildId) return false;
      if (ChannelId != other.ChannelId) return false;
      if (MessageId != other.MessageId) return false;
      if (ObserveToken != other.ObserveToken) return false;
      if (Name != other.Name) return false;
      if (ConnectedMemberCount != other.ConnectedMemberCount) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (GuildId != 0L) hash ^= GuildId.GetHashCode();
      if (ChannelId != 0L) hash ^= ChannelId.GetHashCode();
      if (MessageId != 0L) hash ^= MessageId.GetHashCode();
      if (ObserveToken != 0) hash ^= ObserveToken.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (ConnectedMemberCount != 0) hash ^= ConnectedMemberCount.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (GuildId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(GuildId);
      }
      if (ChannelId != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(ChannelId);
      }
      if (MessageId != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(MessageId);
      }
      if (ObserveToken != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(ObserveToken);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Name);
      }
      if (ConnectedMemberCount != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(ConnectedMemberCount);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (GuildId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(GuildId);
      }
      if (ChannelId != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(ChannelId);
      }
      if (MessageId != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(MessageId);
      }
      if (ObserveToken != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(ObserveToken);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Name);
      }
      if (ConnectedMemberCount != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(ConnectedMemberCount);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (GuildId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(GuildId);
      }
      if (ChannelId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(ChannelId);
      }
      if (MessageId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(MessageId);
      }
      if (ObserveToken != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ObserveToken);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (ConnectedMemberCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ConnectedMemberCount);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(GuildReply other) {
      if (other == null) {
        return;
      }
      if (other.GuildId != 0L) {
        GuildId = other.GuildId;
      }
      if (other.ChannelId != 0L) {
        ChannelId = other.ChannelId;
      }
      if (other.MessageId != 0L) {
        MessageId = other.MessageId;
      }
      if (other.ObserveToken != 0) {
        ObserveToken = other.ObserveToken;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.ConnectedMemberCount != 0) {
        ConnectedMemberCount = other.ConnectedMemberCount;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            GuildId = input.ReadInt64();
            break;
          }
          case 16: {
            ChannelId = input.ReadInt64();
            break;
          }
          case 24: {
            MessageId = input.ReadInt64();
            break;
          }
          case 32: {
            ObserveToken = input.ReadInt32();
            break;
          }
          case 42: {
            Name = input.ReadString();
            break;
          }
          case 48: {
            ConnectedMemberCount = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            GuildId = input.ReadInt64();
            break;
          }
          case 16: {
            ChannelId = input.ReadInt64();
            break;
          }
          case 24: {
            MessageId = input.ReadInt64();
            break;
          }
          case 32: {
            ObserveToken = input.ReadInt32();
            break;
          }
          case 42: {
            Name = input.ReadString();
            break;
          }
          case 48: {
            ConnectedMemberCount = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// Request containing a guild observe token
  /// </summary>
  public sealed partial class GetGuildRequest : pb::IMessage<GetGuildRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetGuildRequest> _parser = new pb::MessageParser<GetGuildRequest>(() => new GetGuildRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<GetGuildRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.GuildsReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildRequest(GetGuildRequest other) : this() {
      observeToken_ = other.observeToken_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildRequest Clone() {
      return new GetGuildRequest(this);
    }

    /// <summary>Field number for the "observeToken" field.</summary>
    public const int ObserveTokenFieldNumber = 1;
    private int observeToken_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ObserveToken {
      get { return observeToken_; }
      set {
        observeToken_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as GetGuildRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(GetGuildRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ObserveToken != other.ObserveToken) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ObserveToken != 0) hash ^= ObserveToken.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ObserveToken != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ObserveToken);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ObserveToken != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ObserveToken);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (ObserveToken != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ObserveToken);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(GetGuildRequest other) {
      if (other == null) {
        return;
      }
      if (other.ObserveToken != 0) {
        ObserveToken = other.ObserveToken;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ObserveToken = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ObserveToken = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// Request containing a guild discord ID
  /// </summary>
  public sealed partial class GetGuildByIdMessage : pb::IMessage<GetGuildByIdMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetGuildByIdMessage> _parser = new pb::MessageParser<GetGuildByIdMessage>(() => new GetGuildByIdMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<GetGuildByIdMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.GuildsReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildByIdMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildByIdMessage(GetGuildByIdMessage other) : this() {
      discordId_ = other.discordId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GetGuildByIdMessage Clone() {
      return new GetGuildByIdMessage(this);
    }

    /// <summary>Field number for the "discordId" field.</summary>
    public const int DiscordIdFieldNumber = 1;
    private long discordId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long DiscordId {
      get { return discordId_; }
      set {
        discordId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as GetGuildByIdMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(GetGuildByIdMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DiscordId != other.DiscordId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (DiscordId != 0L) hash ^= DiscordId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (DiscordId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(DiscordId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (DiscordId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(DiscordId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (DiscordId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(DiscordId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(GetGuildByIdMessage other) {
      if (other == null) {
        return;
      }
      if (other.DiscordId != 0L) {
        DiscordId = other.DiscordId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            DiscordId = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            DiscordId = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
