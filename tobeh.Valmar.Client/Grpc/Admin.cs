// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: admin.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace tobeh.Valmar {

  /// <summary>Holder for reflection information generated from admin.proto</summary>
  public static partial class AdminReflection {

    #region Descriptor
    /// <summary>File descriptor for admin.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AdminReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgthZG1pbi5wcm90bxIFYWRtaW4aG2dvb2dsZS9wcm90b2J1Zi9lbXB0eS5w",
            "cm90bxoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBwZXJzLnByb3RvImIKGFVwZGF0",
            "ZU1lbWJlckZsYWdzUmVxdWVzdBIRCgltZW1iZXJJZHMYASADKAMSFAoMaW52",
            "ZXJ0T3RoZXJzGAIgASgIEg0KBXN0YXRlGAMgASgIEg4KBmZsYWdJZBgEIAEo",
            "BSJAChVTZXRPbmxpbmVJdGVtc1JlcXVlc3QSJwoFaXRlbXMYASADKAsyGC5h",
            "ZG1pbi5PbmxpbmVJdGVtTWVzc2FnZSKDAQoRT25saW5lSXRlbU1lc3NhZ2US",
            "JwoIaXRlbVR5cGUYASABKA4yFS5hZG1pbi5PbmxpbmVJdGVtVHlwZRIMCgRz",
            "bG90GAIgASgFEg4KBml0ZW1JZBgDIAEoBRIQCghsb2JieUtleRgEIAEoCRIV",
            "Cg1sb2JieVBsYXllcklkGAUgASgFIjUKHUluY3JlbWVudE1lbWJlckJ1YmJs",
            "ZXNSZXF1ZXN0EhQKDG1lbWJlckxvZ2lucxgBIAMoBSpgCg5PbmxpbmVJdGVt",
            "VHlwZRIKCgZTcHJpdGUQABIOCgpDb2xvclNoaWZ0EAESCQoFU2NlbmUQAhIJ",
            "CgVBd2FyZBADEgwKCFJld2FyZGVlEAUSDgoKU2NlbmVUaGVtZRAGMsgDCgVB",
            "ZG1pbhJGChRSZWV2YWx1YXRlRHJvcENodW5rcxIWLmdvb2dsZS5wcm90b2J1",
            "Zi5FbXB0eRoWLmdvb2dsZS5wcm90b2J1Zi5FbXB0eRJMChFVcGRhdGVNZW1i",
            "ZXJGbGFncxIfLmFkbWluLlVwZGF0ZU1lbWJlckZsYWdzUmVxdWVzdBoWLmdv",
            "b2dsZS5wcm90b2J1Zi5FbXB0eRJEChJDcmVhdGVCdWJibGVUcmFjZXMSFi5n",
            "b29nbGUucHJvdG9idWYuRW1wdHkaFi5nb29nbGUucHJvdG9idWYuRW1wdHkS",
            "QwoRQ2xlYXJWb2xhdGlsZURhdGESFi5nb29nbGUucHJvdG9idWYuRW1wdHka",
            "Fi5nb29nbGUucHJvdG9idWYuRW1wdHkSVgoWSW5jcmVtZW50TWVtYmVyQnVi",
            "YmxlcxIkLmFkbWluLkluY3JlbWVudE1lbWJlckJ1YmJsZXNSZXF1ZXN0GhYu",
            "Z29vZ2xlLnByb3RvYnVmLkVtcHR5EkYKDlNldE9ubGluZUl0ZW1zEhwuYWRt",
            "aW4uU2V0T25saW5lSXRlbXNSZXF1ZXN0GhYuZ29vZ2xlLnByb3RvYnVmLkVt",
            "cHR5Qg+qAgx0b2JlaC5WYWxtYXJiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::tobeh.Valmar.OnlineItemType), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.UpdateMemberFlagsRequest), global::tobeh.Valmar.UpdateMemberFlagsRequest.Parser, new[]{ "MemberIds", "InvertOthers", "State", "FlagId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.SetOnlineItemsRequest), global::tobeh.Valmar.SetOnlineItemsRequest.Parser, new[]{ "Items" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.OnlineItemMessage), global::tobeh.Valmar.OnlineItemMessage.Parser, new[]{ "ItemType", "Slot", "ItemId", "LobbyKey", "LobbyPlayerId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.IncrementMemberBubblesRequest), global::tobeh.Valmar.IncrementMemberBubblesRequest.Parser, new[]{ "MemberLogins" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum OnlineItemType {
    [pbr::OriginalName("Sprite")] Sprite = 0,
    [pbr::OriginalName("ColorShift")] ColorShift = 1,
    [pbr::OriginalName("Scene")] Scene = 2,
    [pbr::OriginalName("Award")] Award = 3,
    [pbr::OriginalName("Rewardee")] Rewardee = 5,
    [pbr::OriginalName("SceneTheme")] SceneTheme = 6,
  }

  #endregion

  #region Messages
  /// <summary>
  /// Request to update the flags of members
  /// </summary>
  public sealed partial class UpdateMemberFlagsRequest : pb::IMessage<UpdateMemberFlagsRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<UpdateMemberFlagsRequest> _parser = new pb::MessageParser<UpdateMemberFlagsRequest>(() => new UpdateMemberFlagsRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<UpdateMemberFlagsRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.AdminReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public UpdateMemberFlagsRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public UpdateMemberFlagsRequest(UpdateMemberFlagsRequest other) : this() {
      memberIds_ = other.memberIds_.Clone();
      invertOthers_ = other.invertOthers_;
      state_ = other.state_;
      flagId_ = other.flagId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public UpdateMemberFlagsRequest Clone() {
      return new UpdateMemberFlagsRequest(this);
    }

    /// <summary>Field number for the "memberIds" field.</summary>
    public const int MemberIdsFieldNumber = 1;
    private static readonly pb::FieldCodec<long> _repeated_memberIds_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> memberIds_ = new pbc::RepeatedField<long>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<long> MemberIds {
      get { return memberIds_; }
    }

    /// <summary>Field number for the "invertOthers" field.</summary>
    public const int InvertOthersFieldNumber = 2;
    private bool invertOthers_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool InvertOthers {
      get { return invertOthers_; }
      set {
        invertOthers_ = value;
      }
    }

    /// <summary>Field number for the "state" field.</summary>
    public const int StateFieldNumber = 3;
    private bool state_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    /// <summary>Field number for the "flagId" field.</summary>
    public const int FlagIdFieldNumber = 4;
    private int flagId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int FlagId {
      get { return flagId_; }
      set {
        flagId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as UpdateMemberFlagsRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(UpdateMemberFlagsRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!memberIds_.Equals(other.memberIds_)) return false;
      if (InvertOthers != other.InvertOthers) return false;
      if (State != other.State) return false;
      if (FlagId != other.FlagId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= memberIds_.GetHashCode();
      if (InvertOthers != false) hash ^= InvertOthers.GetHashCode();
      if (State != false) hash ^= State.GetHashCode();
      if (FlagId != 0) hash ^= FlagId.GetHashCode();
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
      memberIds_.WriteTo(output, _repeated_memberIds_codec);
      if (InvertOthers != false) {
        output.WriteRawTag(16);
        output.WriteBool(InvertOthers);
      }
      if (State != false) {
        output.WriteRawTag(24);
        output.WriteBool(State);
      }
      if (FlagId != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(FlagId);
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
      memberIds_.WriteTo(ref output, _repeated_memberIds_codec);
      if (InvertOthers != false) {
        output.WriteRawTag(16);
        output.WriteBool(InvertOthers);
      }
      if (State != false) {
        output.WriteRawTag(24);
        output.WriteBool(State);
      }
      if (FlagId != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(FlagId);
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
      size += memberIds_.CalculateSize(_repeated_memberIds_codec);
      if (InvertOthers != false) {
        size += 1 + 1;
      }
      if (State != false) {
        size += 1 + 1;
      }
      if (FlagId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(FlagId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(UpdateMemberFlagsRequest other) {
      if (other == null) {
        return;
      }
      memberIds_.Add(other.memberIds_);
      if (other.InvertOthers != false) {
        InvertOthers = other.InvertOthers;
      }
      if (other.State != false) {
        State = other.State;
      }
      if (other.FlagId != 0) {
        FlagId = other.FlagId;
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
          case 10:
          case 8: {
            memberIds_.AddEntriesFrom(input, _repeated_memberIds_codec);
            break;
          }
          case 16: {
            InvertOthers = input.ReadBool();
            break;
          }
          case 24: {
            State = input.ReadBool();
            break;
          }
          case 32: {
            FlagId = input.ReadInt32();
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
          case 10:
          case 8: {
            memberIds_.AddEntriesFrom(ref input, _repeated_memberIds_codec);
            break;
          }
          case 16: {
            InvertOthers = input.ReadBool();
            break;
          }
          case 24: {
            State = input.ReadBool();
            break;
          }
          case 32: {
            FlagId = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SetOnlineItemsRequest : pb::IMessage<SetOnlineItemsRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SetOnlineItemsRequest> _parser = new pb::MessageParser<SetOnlineItemsRequest>(() => new SetOnlineItemsRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<SetOnlineItemsRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.AdminReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetOnlineItemsRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetOnlineItemsRequest(SetOnlineItemsRequest other) : this() {
      items_ = other.items_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetOnlineItemsRequest Clone() {
      return new SetOnlineItemsRequest(this);
    }

    /// <summary>Field number for the "items" field.</summary>
    public const int ItemsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::tobeh.Valmar.OnlineItemMessage> _repeated_items_codec
        = pb::FieldCodec.ForMessage(10, global::tobeh.Valmar.OnlineItemMessage.Parser);
    private readonly pbc::RepeatedField<global::tobeh.Valmar.OnlineItemMessage> items_ = new pbc::RepeatedField<global::tobeh.Valmar.OnlineItemMessage>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::tobeh.Valmar.OnlineItemMessage> Items {
      get { return items_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as SetOnlineItemsRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(SetOnlineItemsRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!items_.Equals(other.items_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= items_.GetHashCode();
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
      items_.WriteTo(output, _repeated_items_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      items_.WriteTo(ref output, _repeated_items_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += items_.CalculateSize(_repeated_items_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(SetOnlineItemsRequest other) {
      if (other == null) {
        return;
      }
      items_.Add(other.items_);
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
          case 10: {
            items_.AddEntriesFrom(input, _repeated_items_codec);
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
          case 10: {
            items_.AddEntriesFrom(ref input, _repeated_items_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class OnlineItemMessage : pb::IMessage<OnlineItemMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<OnlineItemMessage> _parser = new pb::MessageParser<OnlineItemMessage>(() => new OnlineItemMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<OnlineItemMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.AdminReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OnlineItemMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OnlineItemMessage(OnlineItemMessage other) : this() {
      itemType_ = other.itemType_;
      slot_ = other.slot_;
      itemId_ = other.itemId_;
      lobbyKey_ = other.lobbyKey_;
      lobbyPlayerId_ = other.lobbyPlayerId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OnlineItemMessage Clone() {
      return new OnlineItemMessage(this);
    }

    /// <summary>Field number for the "itemType" field.</summary>
    public const int ItemTypeFieldNumber = 1;
    private global::tobeh.Valmar.OnlineItemType itemType_ = global::tobeh.Valmar.OnlineItemType.Sprite;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::tobeh.Valmar.OnlineItemType ItemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    /// <summary>Field number for the "slot" field.</summary>
    public const int SlotFieldNumber = 2;
    private int slot_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Slot {
      get { return slot_; }
      set {
        slot_ = value;
      }
    }

    /// <summary>Field number for the "itemId" field.</summary>
    public const int ItemIdFieldNumber = 3;
    private int itemId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "lobbyKey" field.</summary>
    public const int LobbyKeyFieldNumber = 4;
    private string lobbyKey_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string LobbyKey {
      get { return lobbyKey_; }
      set {
        lobbyKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "lobbyPlayerId" field.</summary>
    public const int LobbyPlayerIdFieldNumber = 5;
    private int lobbyPlayerId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int LobbyPlayerId {
      get { return lobbyPlayerId_; }
      set {
        lobbyPlayerId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as OnlineItemMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(OnlineItemMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ItemType != other.ItemType) return false;
      if (Slot != other.Slot) return false;
      if (ItemId != other.ItemId) return false;
      if (LobbyKey != other.LobbyKey) return false;
      if (LobbyPlayerId != other.LobbyPlayerId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ItemType != global::tobeh.Valmar.OnlineItemType.Sprite) hash ^= ItemType.GetHashCode();
      if (Slot != 0) hash ^= Slot.GetHashCode();
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
      if (LobbyKey.Length != 0) hash ^= LobbyKey.GetHashCode();
      if (LobbyPlayerId != 0) hash ^= LobbyPlayerId.GetHashCode();
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
      if (ItemType != global::tobeh.Valmar.OnlineItemType.Sprite) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ItemType);
      }
      if (Slot != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Slot);
      }
      if (ItemId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(ItemId);
      }
      if (LobbyKey.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(LobbyKey);
      }
      if (LobbyPlayerId != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(LobbyPlayerId);
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
      if (ItemType != global::tobeh.Valmar.OnlineItemType.Sprite) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ItemType);
      }
      if (Slot != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Slot);
      }
      if (ItemId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(ItemId);
      }
      if (LobbyKey.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(LobbyKey);
      }
      if (LobbyPlayerId != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(LobbyPlayerId);
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
      if (ItemType != global::tobeh.Valmar.OnlineItemType.Sprite) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ItemType);
      }
      if (Slot != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Slot);
      }
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ItemId);
      }
      if (LobbyKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(LobbyKey);
      }
      if (LobbyPlayerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(LobbyPlayerId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(OnlineItemMessage other) {
      if (other == null) {
        return;
      }
      if (other.ItemType != global::tobeh.Valmar.OnlineItemType.Sprite) {
        ItemType = other.ItemType;
      }
      if (other.Slot != 0) {
        Slot = other.Slot;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
      }
      if (other.LobbyKey.Length != 0) {
        LobbyKey = other.LobbyKey;
      }
      if (other.LobbyPlayerId != 0) {
        LobbyPlayerId = other.LobbyPlayerId;
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
            ItemType = (global::tobeh.Valmar.OnlineItemType) input.ReadEnum();
            break;
          }
          case 16: {
            Slot = input.ReadInt32();
            break;
          }
          case 24: {
            ItemId = input.ReadInt32();
            break;
          }
          case 34: {
            LobbyKey = input.ReadString();
            break;
          }
          case 40: {
            LobbyPlayerId = input.ReadInt32();
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
            ItemType = (global::tobeh.Valmar.OnlineItemType) input.ReadEnum();
            break;
          }
          case 16: {
            Slot = input.ReadInt32();
            break;
          }
          case 24: {
            ItemId = input.ReadInt32();
            break;
          }
          case 34: {
            LobbyKey = input.ReadString();
            break;
          }
          case 40: {
            LobbyPlayerId = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// Request to increment the bubble count of a range of members
  /// </summary>
  public sealed partial class IncrementMemberBubblesRequest : pb::IMessage<IncrementMemberBubblesRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<IncrementMemberBubblesRequest> _parser = new pb::MessageParser<IncrementMemberBubblesRequest>(() => new IncrementMemberBubblesRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<IncrementMemberBubblesRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.AdminReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncrementMemberBubblesRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncrementMemberBubblesRequest(IncrementMemberBubblesRequest other) : this() {
      memberLogins_ = other.memberLogins_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncrementMemberBubblesRequest Clone() {
      return new IncrementMemberBubblesRequest(this);
    }

    /// <summary>Field number for the "memberLogins" field.</summary>
    public const int MemberLoginsFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_memberLogins_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> memberLogins_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<int> MemberLogins {
      get { return memberLogins_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as IncrementMemberBubblesRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(IncrementMemberBubblesRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!memberLogins_.Equals(other.memberLogins_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= memberLogins_.GetHashCode();
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
      memberLogins_.WriteTo(output, _repeated_memberLogins_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      memberLogins_.WriteTo(ref output, _repeated_memberLogins_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += memberLogins_.CalculateSize(_repeated_memberLogins_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(IncrementMemberBubblesRequest other) {
      if (other == null) {
        return;
      }
      memberLogins_.Add(other.memberLogins_);
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
          case 10:
          case 8: {
            memberLogins_.AddEntriesFrom(input, _repeated_memberLogins_codec);
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
          case 10:
          case 8: {
            memberLogins_.AddEntriesFrom(ref input, _repeated_memberLogins_codec);
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
