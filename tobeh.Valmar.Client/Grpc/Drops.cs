// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: drops.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace tobeh.Valmar {

  /// <summary>Holder for reflection information generated from drops.proto</summary>
  public static partial class DropsReflection {

    #region Descriptor
    /// <summary>File descriptor for drops.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DropsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cgtkcm9wcy5wcm90bxIFYWRtaW4aG2dvb2dsZS9wcm90b2J1Zi9lbXB0eS5w",
            "cm90bxoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBwZXJzLnByb3RvIl0KE1NjaGVk",
            "dWxlRHJvcFJlcXVlc3QSFAoMZGVsYXlTZWNvbmRzGAEgASgFEjAKC2V2ZW50",
            "RHJvcElkGAIgASgLMhsuZ29vZ2xlLnByb3RvYnVmLkludDMyVmFsdWUiRwoV",
            "Q2FsY3VsYXRlRGVsYXlSZXF1ZXN0EhkKEW9ubGluZVBsYXllckNvdW50GAEg",
            "ASgFEhMKC2Jvb3N0RmFjdG9yGAIgASgBIkgKFERyb3BEZWxheUJvdW5kc1Jl",
            "cGx5EhcKD21pbkRlbGF5U2Vjb25kcxgBIAEoBRIXCg9tYXhEZWxheVNlY29u",
            "ZHMYAiABKAUiKAoXQ3VycmVudEJvb3N0RmFjdG9yUmVwbHkSDQoFYm9vc3QY",
            "ASABKAEy8wEKBURyb3BzEkIKDFNjaGVkdWxlRHJvcBIaLmFkbWluLlNjaGVk",
            "dWxlRHJvcFJlcXVlc3QaFi5nb29nbGUucHJvdG9idWYuRW1wdHkSTwoVR2V0",
            "Q3VycmVudEJvb3N0RmFjdG9yEhYuZ29vZ2xlLnByb3RvYnVmLkVtcHR5Gh4u",
            "YWRtaW4uQ3VycmVudEJvb3N0RmFjdG9yUmVwbHkSVQoYQ2FsY3VsYXRlRHJv",
            "cERlbGF5Qm91bmRzEhwuYWRtaW4uQ2FsY3VsYXRlRGVsYXlSZXF1ZXN0Ghsu",
            "YWRtaW4uRHJvcERlbGF5Qm91bmRzUmVwbHlCD6oCDHRvYmVoLlZhbG1hcmIG",
            "cHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.ScheduleDropRequest), global::tobeh.Valmar.ScheduleDropRequest.Parser, new[]{ "DelaySeconds", "EventDropId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.CalculateDelayRequest), global::tobeh.Valmar.CalculateDelayRequest.Parser, new[]{ "OnlinePlayerCount", "BoostFactor" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.DropDelayBoundsReply), global::tobeh.Valmar.DropDelayBoundsReply.Parser, new[]{ "MinDelaySeconds", "MaxDelaySeconds" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::tobeh.Valmar.CurrentBoostFactorReply), global::tobeh.Valmar.CurrentBoostFactorReply.Parser, new[]{ "Boost" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// request to schedule a new drop
  /// </summary>
  public sealed partial class ScheduleDropRequest : pb::IMessage<ScheduleDropRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ScheduleDropRequest> _parser = new pb::MessageParser<ScheduleDropRequest>(() => new ScheduleDropRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ScheduleDropRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.DropsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ScheduleDropRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ScheduleDropRequest(ScheduleDropRequest other) : this() {
      delaySeconds_ = other.delaySeconds_;
      EventDropId = other.EventDropId;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ScheduleDropRequest Clone() {
      return new ScheduleDropRequest(this);
    }

    /// <summary>Field number for the "delaySeconds" field.</summary>
    public const int DelaySecondsFieldNumber = 1;
    private int delaySeconds_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int DelaySeconds {
      get { return delaySeconds_; }
      set {
        delaySeconds_ = value;
      }
    }

    /// <summary>Field number for the "eventDropId" field.</summary>
    public const int EventDropIdFieldNumber = 2;
    private static readonly pb::FieldCodec<int?> _single_eventDropId_codec = pb::FieldCodec.ForStructWrapper<int>(18);
    private int? eventDropId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int? EventDropId {
      get { return eventDropId_; }
      set {
        eventDropId_ = value;
      }
    }


    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ScheduleDropRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ScheduleDropRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DelaySeconds != other.DelaySeconds) return false;
      if (EventDropId != other.EventDropId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (DelaySeconds != 0) hash ^= DelaySeconds.GetHashCode();
      if (eventDropId_ != null) hash ^= EventDropId.GetHashCode();
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
      if (DelaySeconds != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(DelaySeconds);
      }
      if (eventDropId_ != null) {
        _single_eventDropId_codec.WriteTagAndValue(output, EventDropId);
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
      if (DelaySeconds != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(DelaySeconds);
      }
      if (eventDropId_ != null) {
        _single_eventDropId_codec.WriteTagAndValue(ref output, EventDropId);
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
      if (DelaySeconds != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DelaySeconds);
      }
      if (eventDropId_ != null) {
        size += _single_eventDropId_codec.CalculateSizeWithTag(EventDropId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ScheduleDropRequest other) {
      if (other == null) {
        return;
      }
      if (other.DelaySeconds != 0) {
        DelaySeconds = other.DelaySeconds;
      }
      if (other.eventDropId_ != null) {
        if (eventDropId_ == null || other.EventDropId != 0) {
          EventDropId = other.EventDropId;
        }
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
            DelaySeconds = input.ReadInt32();
            break;
          }
          case 18: {
            int? value = _single_eventDropId_codec.Read(input);
            if (eventDropId_ == null || value != 0) {
              EventDropId = value;
            }
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
            DelaySeconds = input.ReadInt32();
            break;
          }
          case 18: {
            int? value = _single_eventDropId_codec.Read(ref input);
            if (eventDropId_ == null || value != 0) {
              EventDropId = value;
            }
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// message to calculate the delay bounds for a drop
  /// </summary>
  public sealed partial class CalculateDelayRequest : pb::IMessage<CalculateDelayRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CalculateDelayRequest> _parser = new pb::MessageParser<CalculateDelayRequest>(() => new CalculateDelayRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CalculateDelayRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.DropsReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CalculateDelayRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CalculateDelayRequest(CalculateDelayRequest other) : this() {
      onlinePlayerCount_ = other.onlinePlayerCount_;
      boostFactor_ = other.boostFactor_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CalculateDelayRequest Clone() {
      return new CalculateDelayRequest(this);
    }

    /// <summary>Field number for the "onlinePlayerCount" field.</summary>
    public const int OnlinePlayerCountFieldNumber = 1;
    private int onlinePlayerCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int OnlinePlayerCount {
      get { return onlinePlayerCount_; }
      set {
        onlinePlayerCount_ = value;
      }
    }

    /// <summary>Field number for the "boostFactor" field.</summary>
    public const int BoostFactorFieldNumber = 2;
    private double boostFactor_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double BoostFactor {
      get { return boostFactor_; }
      set {
        boostFactor_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CalculateDelayRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CalculateDelayRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (OnlinePlayerCount != other.OnlinePlayerCount) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BoostFactor, other.BoostFactor)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (OnlinePlayerCount != 0) hash ^= OnlinePlayerCount.GetHashCode();
      if (BoostFactor != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BoostFactor);
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
      if (OnlinePlayerCount != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(OnlinePlayerCount);
      }
      if (BoostFactor != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(BoostFactor);
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
      if (OnlinePlayerCount != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(OnlinePlayerCount);
      }
      if (BoostFactor != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(BoostFactor);
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
      if (OnlinePlayerCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(OnlinePlayerCount);
      }
      if (BoostFactor != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CalculateDelayRequest other) {
      if (other == null) {
        return;
      }
      if (other.OnlinePlayerCount != 0) {
        OnlinePlayerCount = other.OnlinePlayerCount;
      }
      if (other.BoostFactor != 0D) {
        BoostFactor = other.BoostFactor;
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
            OnlinePlayerCount = input.ReadInt32();
            break;
          }
          case 17: {
            BoostFactor = input.ReadDouble();
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
            OnlinePlayerCount = input.ReadInt32();
            break;
          }
          case 17: {
            BoostFactor = input.ReadDouble();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// response containing the min and max delay bounds for a drop delay
  /// </summary>
  public sealed partial class DropDelayBoundsReply : pb::IMessage<DropDelayBoundsReply>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<DropDelayBoundsReply> _parser = new pb::MessageParser<DropDelayBoundsReply>(() => new DropDelayBoundsReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<DropDelayBoundsReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.DropsReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DropDelayBoundsReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DropDelayBoundsReply(DropDelayBoundsReply other) : this() {
      minDelaySeconds_ = other.minDelaySeconds_;
      maxDelaySeconds_ = other.maxDelaySeconds_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DropDelayBoundsReply Clone() {
      return new DropDelayBoundsReply(this);
    }

    /// <summary>Field number for the "minDelaySeconds" field.</summary>
    public const int MinDelaySecondsFieldNumber = 1;
    private int minDelaySeconds_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int MinDelaySeconds {
      get { return minDelaySeconds_; }
      set {
        minDelaySeconds_ = value;
      }
    }

    /// <summary>Field number for the "maxDelaySeconds" field.</summary>
    public const int MaxDelaySecondsFieldNumber = 2;
    private int maxDelaySeconds_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int MaxDelaySeconds {
      get { return maxDelaySeconds_; }
      set {
        maxDelaySeconds_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as DropDelayBoundsReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(DropDelayBoundsReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MinDelaySeconds != other.MinDelaySeconds) return false;
      if (MaxDelaySeconds != other.MaxDelaySeconds) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (MinDelaySeconds != 0) hash ^= MinDelaySeconds.GetHashCode();
      if (MaxDelaySeconds != 0) hash ^= MaxDelaySeconds.GetHashCode();
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
      if (MinDelaySeconds != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(MinDelaySeconds);
      }
      if (MaxDelaySeconds != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(MaxDelaySeconds);
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
      if (MinDelaySeconds != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(MinDelaySeconds);
      }
      if (MaxDelaySeconds != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(MaxDelaySeconds);
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
      if (MinDelaySeconds != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MinDelaySeconds);
      }
      if (MaxDelaySeconds != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MaxDelaySeconds);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(DropDelayBoundsReply other) {
      if (other == null) {
        return;
      }
      if (other.MinDelaySeconds != 0) {
        MinDelaySeconds = other.MinDelaySeconds;
      }
      if (other.MaxDelaySeconds != 0) {
        MaxDelaySeconds = other.MaxDelaySeconds;
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
            MinDelaySeconds = input.ReadInt32();
            break;
          }
          case 16: {
            MaxDelaySeconds = input.ReadInt32();
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
            MinDelaySeconds = input.ReadInt32();
            break;
          }
          case 16: {
            MaxDelaySeconds = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// request to get the current boost factor
  /// </summary>
  public sealed partial class CurrentBoostFactorReply : pb::IMessage<CurrentBoostFactorReply>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CurrentBoostFactorReply> _parser = new pb::MessageParser<CurrentBoostFactorReply>(() => new CurrentBoostFactorReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CurrentBoostFactorReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::tobeh.Valmar.DropsReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CurrentBoostFactorReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CurrentBoostFactorReply(CurrentBoostFactorReply other) : this() {
      boost_ = other.boost_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CurrentBoostFactorReply Clone() {
      return new CurrentBoostFactorReply(this);
    }

    /// <summary>Field number for the "boost" field.</summary>
    public const int BoostFieldNumber = 1;
    private double boost_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double Boost {
      get { return boost_; }
      set {
        boost_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CurrentBoostFactorReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CurrentBoostFactorReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Boost, other.Boost)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Boost != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Boost);
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
      if (Boost != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Boost);
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
      if (Boost != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Boost);
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
      if (Boost != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CurrentBoostFactorReply other) {
      if (other == null) {
        return;
      }
      if (other.Boost != 0D) {
        Boost = other.Boost;
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
          case 9: {
            Boost = input.ReadDouble();
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
          case 9: {
            Boost = input.ReadDouble();
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
