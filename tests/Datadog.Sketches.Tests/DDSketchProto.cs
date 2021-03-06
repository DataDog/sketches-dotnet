// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: DDSketch.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Datadog.Sketches.Tests.DDSketchProto {

  /// <summary>Holder for reflection information generated from DDSketch.proto</summary>
  public static partial class DDSketchReflection {

    #region Descriptor
    /// <summary>File descriptor for DDSketch.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DDSketchReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5ERFNrZXRjaC5wcm90byJ9CghERFNrZXRjaBIeCgdtYXBwaW5nGAEgASgL",
            "Mg0uSW5kZXhNYXBwaW5nEh4KDnBvc2l0aXZlVmFsdWVzGAIgASgLMgYuU3Rv",
            "cmUSHgoObmVnYXRpdmVWYWx1ZXMYAyABKAsyBi5TdG9yZRIRCgl6ZXJvQ291",
            "bnQYBCABKAEitAEKDEluZGV4TWFwcGluZxINCgVnYW1tYRgBIAEoARITCgtp",
            "bmRleE9mZnNldBgCIAEoARIyCg1pbnRlcnBvbGF0aW9uGAMgASgOMhsuSW5k",
            "ZXhNYXBwaW5nLkludGVycG9sYXRpb24iTAoNSW50ZXJwb2xhdGlvbhIICgRO",
            "T05FEAASCgoGTElORUFSEAESDQoJUVVBRFJBVElDEAISCQoFQ1VCSUMQAxIL",
            "CgdRVUFSVElDEAQipgEKBVN0b3JlEigKCWJpbkNvdW50cxgBIAMoCzIVLlN0",
            "b3JlLkJpbkNvdW50c0VudHJ5Eh8KE2NvbnRpZ3VvdXNCaW5Db3VudHMYAiAD",
            "KAFCAhABEiAKGGNvbnRpZ3VvdXNCaW5JbmRleE9mZnNldBgDIAEoERowCg5C",
            "aW5Db3VudHNFbnRyeRILCgNrZXkYASABKBESDQoFdmFsdWUYAiABKAE6AjgB",
            "QieqAiREYXRhZG9nLlNrZXRjaGVzLlRlc3RzLkREU2tldGNoUHJvdG9iBnBy",
            "b3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Datadog.Sketches.Tests.DDSketchProto.DDSketchProto), global::Datadog.Sketches.Tests.DDSketchProto.DDSketchProto.Parser, new[]{ "Mapping", "PositiveValues", "NegativeValues", "ZeroCount" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping), global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Parser, new[]{ "Gamma", "IndexOffset", "Interpolation" }, null, new[]{ typeof(global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation) }, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Datadog.Sketches.Tests.DDSketchProto.Store), global::Datadog.Sketches.Tests.DDSketchProto.Store.Parser, new[]{ "BinCounts", "ContiguousBinCounts", "ContiguousBinIndexOffset" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// A DDSketch is essentially a histogram that partitions the range of positive values into an infinite number of
  /// indexed bins whose size grows exponentially. It keeps track of the number of values (or possibly floating-point
  /// weights) added to each bin. Negative values are partitioned like positive values, symmetrically to zero.
  /// The value zero as well as its close neighborhood that would be mapped to extreme bin indexes is mapped to a specific
  /// counter.
  /// </summary>
  public sealed partial class DDSketchProto : pb::IMessage<DDSketchProto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<DDSketchProto> _parser = new pb::MessageParser<DDSketchProto>(() => new DDSketchProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<DDSketchProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Datadog.Sketches.Tests.DDSketchProto.DDSketchReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DDSketchProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DDSketchProto(DDSketchProto other) : this() {
      mapping_ = other.mapping_ != null ? other.mapping_.Clone() : null;
      positiveValues_ = other.positiveValues_ != null ? other.positiveValues_.Clone() : null;
      negativeValues_ = other.negativeValues_ != null ? other.negativeValues_.Clone() : null;
      zeroCount_ = other.zeroCount_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public DDSketchProto Clone() {
      return new DDSketchProto(this);
    }

    /// <summary>Field number for the "mapping" field.</summary>
    public const int MappingFieldNumber = 1;
    private global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping mapping_;
    /// <summary>
    /// The mapping between positive values and the bin indexes they belong to.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping Mapping {
      get { return mapping_; }
      set {
        mapping_ = value;
      }
    }

    /// <summary>Field number for the "positiveValues" field.</summary>
    public const int PositiveValuesFieldNumber = 2;
    private global::Datadog.Sketches.Tests.DDSketchProto.Store positiveValues_;
    /// <summary>
    /// The store for keeping track of positive values.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Datadog.Sketches.Tests.DDSketchProto.Store PositiveValues {
      get { return positiveValues_; }
      set {
        positiveValues_ = value;
      }
    }

    /// <summary>Field number for the "negativeValues" field.</summary>
    public const int NegativeValuesFieldNumber = 3;
    private global::Datadog.Sketches.Tests.DDSketchProto.Store negativeValues_;
    /// <summary>
    /// The store for keeping track of negative values. A negative value v is mapped using its positive opposite -v.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Datadog.Sketches.Tests.DDSketchProto.Store NegativeValues {
      get { return negativeValues_; }
      set {
        negativeValues_ = value;
      }
    }

    /// <summary>Field number for the "zeroCount" field.</summary>
    public const int ZeroCountFieldNumber = 4;
    private double zeroCount_;
    /// <summary>
    /// The count for the value zero and its close neighborhood (whose width depends on the mapping).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double ZeroCount {
      get { return zeroCount_; }
      set {
        zeroCount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as DDSketchProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(DDSketchProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Mapping, other.Mapping)) return false;
      if (!object.Equals(PositiveValues, other.PositiveValues)) return false;
      if (!object.Equals(NegativeValues, other.NegativeValues)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ZeroCount, other.ZeroCount)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (mapping_ != null) hash ^= Mapping.GetHashCode();
      if (positiveValues_ != null) hash ^= PositiveValues.GetHashCode();
      if (negativeValues_ != null) hash ^= NegativeValues.GetHashCode();
      if (ZeroCount != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ZeroCount);
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
      if (mapping_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Mapping);
      }
      if (positiveValues_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(PositiveValues);
      }
      if (negativeValues_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(NegativeValues);
      }
      if (ZeroCount != 0D) {
        output.WriteRawTag(33);
        output.WriteDouble(ZeroCount);
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
      if (mapping_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Mapping);
      }
      if (positiveValues_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(PositiveValues);
      }
      if (negativeValues_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(NegativeValues);
      }
      if (ZeroCount != 0D) {
        output.WriteRawTag(33);
        output.WriteDouble(ZeroCount);
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
      if (mapping_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Mapping);
      }
      if (positiveValues_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(PositiveValues);
      }
      if (negativeValues_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(NegativeValues);
      }
      if (ZeroCount != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(DDSketchProto other) {
      if (other == null) {
        return;
      }
      if (other.mapping_ != null) {
        if (mapping_ == null) {
          Mapping = new global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping();
        }
        Mapping.MergeFrom(other.Mapping);
      }
      if (other.positiveValues_ != null) {
        if (positiveValues_ == null) {
          PositiveValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
        }
        PositiveValues.MergeFrom(other.PositiveValues);
      }
      if (other.negativeValues_ != null) {
        if (negativeValues_ == null) {
          NegativeValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
        }
        NegativeValues.MergeFrom(other.NegativeValues);
      }
      if (other.ZeroCount != 0D) {
        ZeroCount = other.ZeroCount;
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
          case 10: {
            if (mapping_ == null) {
              Mapping = new global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping();
            }
            input.ReadMessage(Mapping);
            break;
          }
          case 18: {
            if (positiveValues_ == null) {
              PositiveValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
            }
            input.ReadMessage(PositiveValues);
            break;
          }
          case 26: {
            if (negativeValues_ == null) {
              NegativeValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
            }
            input.ReadMessage(NegativeValues);
            break;
          }
          case 33: {
            ZeroCount = input.ReadDouble();
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
            if (mapping_ == null) {
              Mapping = new global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping();
            }
            input.ReadMessage(Mapping);
            break;
          }
          case 18: {
            if (positiveValues_ == null) {
              PositiveValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
            }
            input.ReadMessage(PositiveValues);
            break;
          }
          case 26: {
            if (negativeValues_ == null) {
              NegativeValues = new global::Datadog.Sketches.Tests.DDSketchProto.Store();
            }
            input.ReadMessage(NegativeValues);
            break;
          }
          case 33: {
            ZeroCount = input.ReadDouble();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// How to map positive values to the bins they belong to.
  /// </summary>
  public sealed partial class IndexMapping : pb::IMessage<IndexMapping>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<IndexMapping> _parser = new pb::MessageParser<IndexMapping>(() => new IndexMapping());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<IndexMapping> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Datadog.Sketches.Tests.DDSketchProto.DDSketchReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IndexMapping() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IndexMapping(IndexMapping other) : this() {
      gamma_ = other.gamma_;
      indexOffset_ = other.indexOffset_;
      interpolation_ = other.interpolation_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IndexMapping Clone() {
      return new IndexMapping(this);
    }

    /// <summary>Field number for the "gamma" field.</summary>
    public const int GammaFieldNumber = 1;
    private double gamma_;
    /// <summary>
    /// The gamma parameter of the mapping, such that bin index that a value v belongs to is roughly equal to
    /// log(v)/log(gamma).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double Gamma {
      get { return gamma_; }
      set {
        gamma_ = value;
      }
    }

    /// <summary>Field number for the "indexOffset" field.</summary>
    public const int IndexOffsetFieldNumber = 2;
    private double indexOffset_;
    /// <summary>
    /// An offset that can be used to shift all bin indexes.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double IndexOffset {
      get { return indexOffset_; }
      set {
        indexOffset_ = value;
      }
    }

    /// <summary>Field number for the "interpolation" field.</summary>
    public const int InterpolationFieldNumber = 3;
    private global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation interpolation_ = global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None;
    /// <summary>
    /// To speed up the computation of the index a value belongs to, the computation of the log may be approximated using
    /// the fact that the log to the base 2 of powers of 2 can be computed at a low cost from the binary representation of
    /// the input value. Other values can be approximated by interpolating between successive powers of 2 (linearly,
    /// quadratically or cubically).
    /// NONE means that the log is to be computed exactly (no interpolation).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation Interpolation {
      get { return interpolation_; }
      set {
        interpolation_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as IndexMapping);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(IndexMapping other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Gamma, other.Gamma)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(IndexOffset, other.IndexOffset)) return false;
      if (Interpolation != other.Interpolation) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Gamma != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Gamma);
      if (IndexOffset != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(IndexOffset);
      if (Interpolation != global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None) hash ^= Interpolation.GetHashCode();
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
      if (Gamma != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Gamma);
      }
      if (IndexOffset != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(IndexOffset);
      }
      if (Interpolation != global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Interpolation);
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
      if (Gamma != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Gamma);
      }
      if (IndexOffset != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(IndexOffset);
      }
      if (Interpolation != global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Interpolation);
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
      if (Gamma != 0D) {
        size += 1 + 8;
      }
      if (IndexOffset != 0D) {
        size += 1 + 8;
      }
      if (Interpolation != global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Interpolation);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(IndexMapping other) {
      if (other == null) {
        return;
      }
      if (other.Gamma != 0D) {
        Gamma = other.Gamma;
      }
      if (other.IndexOffset != 0D) {
        IndexOffset = other.IndexOffset;
      }
      if (other.Interpolation != global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation.None) {
        Interpolation = other.Interpolation;
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
            Gamma = input.ReadDouble();
            break;
          }
          case 17: {
            IndexOffset = input.ReadDouble();
            break;
          }
          case 24: {
            Interpolation = (global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation) input.ReadEnum();
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
            Gamma = input.ReadDouble();
            break;
          }
          case 17: {
            IndexOffset = input.ReadDouble();
            break;
          }
          case 24: {
            Interpolation = (global::Datadog.Sketches.Tests.DDSketchProto.IndexMapping.Types.Interpolation) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the IndexMapping message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum Interpolation {
        [pbr::OriginalName("NONE")] None = 0,
        [pbr::OriginalName("LINEAR")] Linear = 1,
        [pbr::OriginalName("QUADRATIC")] Quadratic = 2,
        [pbr::OriginalName("CUBIC")] Cubic = 3,
        [pbr::OriginalName("QUARTIC")] Quartic = 4,
      }

    }
    #endregion

  }

  /// <summary>
  /// A Store maps bin indexes to their respective counts.
  /// Counts can be encoded sparsely using binCounts, but also in a contiguous way using contiguousBinCounts and
  /// contiguousBinIndexOffset. Given that non-empty bins are in practice usually contiguous or close to one another, the
  /// latter contiguous encoding method is usually more efficient than the sparse one.
  /// Both encoding methods can be used conjointly. If a bin appears in both the sparse and the contiguous encodings, its
  /// count value is the sum of the counts in each encodings.
  /// </summary>
  public sealed partial class Store : pb::IMessage<Store>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Store> _parser = new pb::MessageParser<Store>(() => new Store());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<Store> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Datadog.Sketches.Tests.DDSketchProto.DDSketchReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Store() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Store(Store other) : this() {
      binCounts_ = other.binCounts_.Clone();
      contiguousBinCounts_ = other.contiguousBinCounts_.Clone();
      contiguousBinIndexOffset_ = other.contiguousBinIndexOffset_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Store Clone() {
      return new Store(this);
    }

    /// <summary>Field number for the "binCounts" field.</summary>
    public const int BinCountsFieldNumber = 1;
    private static readonly pbc::MapField<int, double>.Codec _map_binCounts_codec
        = new pbc::MapField<int, double>.Codec(pb::FieldCodec.ForSInt32(8, 0), pb::FieldCodec.ForDouble(17, 0D), 10);
    private readonly pbc::MapField<int, double> binCounts_ = new pbc::MapField<int, double>();
    /// <summary>
    /// The bin counts, encoded sparsely.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::MapField<int, double> BinCounts {
      get { return binCounts_; }
    }

    /// <summary>Field number for the "contiguousBinCounts" field.</summary>
    public const int ContiguousBinCountsFieldNumber = 2;
    private static readonly pb::FieldCodec<double> _repeated_contiguousBinCounts_codec
        = pb::FieldCodec.ForDouble(18);
    private readonly pbc::RepeatedField<double> contiguousBinCounts_ = new pbc::RepeatedField<double>();
    /// <summary>
    /// The bin counts, encoded contiguously. The values of contiguousBinCounts are the counts for the bins of indexes
    /// o, o+1, o+2, etc., where o is contiguousBinIndexOffset.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<double> ContiguousBinCounts {
      get { return contiguousBinCounts_; }
    }

    /// <summary>Field number for the "contiguousBinIndexOffset" field.</summary>
    public const int ContiguousBinIndexOffsetFieldNumber = 3;
    private int contiguousBinIndexOffset_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ContiguousBinIndexOffset {
      get { return contiguousBinIndexOffset_; }
      set {
        contiguousBinIndexOffset_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as Store);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(Store other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!BinCounts.Equals(other.BinCounts)) return false;
      if(!contiguousBinCounts_.Equals(other.contiguousBinCounts_)) return false;
      if (ContiguousBinIndexOffset != other.ContiguousBinIndexOffset) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= BinCounts.GetHashCode();
      hash ^= contiguousBinCounts_.GetHashCode();
      if (ContiguousBinIndexOffset != 0) hash ^= ContiguousBinIndexOffset.GetHashCode();
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
      binCounts_.WriteTo(output, _map_binCounts_codec);
      contiguousBinCounts_.WriteTo(output, _repeated_contiguousBinCounts_codec);
      if (ContiguousBinIndexOffset != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(ContiguousBinIndexOffset);
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
      binCounts_.WriteTo(ref output, _map_binCounts_codec);
      contiguousBinCounts_.WriteTo(ref output, _repeated_contiguousBinCounts_codec);
      if (ContiguousBinIndexOffset != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(ContiguousBinIndexOffset);
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
      size += binCounts_.CalculateSize(_map_binCounts_codec);
      size += contiguousBinCounts_.CalculateSize(_repeated_contiguousBinCounts_codec);
      if (ContiguousBinIndexOffset != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ContiguousBinIndexOffset);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(Store other) {
      if (other == null) {
        return;
      }
      binCounts_.Add(other.binCounts_);
      contiguousBinCounts_.Add(other.contiguousBinCounts_);
      if (other.ContiguousBinIndexOffset != 0) {
        ContiguousBinIndexOffset = other.ContiguousBinIndexOffset;
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
          case 10: {
            binCounts_.AddEntriesFrom(input, _map_binCounts_codec);
            break;
          }
          case 18:
          case 17: {
            contiguousBinCounts_.AddEntriesFrom(input, _repeated_contiguousBinCounts_codec);
            break;
          }
          case 24: {
            ContiguousBinIndexOffset = input.ReadSInt32();
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
            binCounts_.AddEntriesFrom(ref input, _map_binCounts_codec);
            break;
          }
          case 18:
          case 17: {
            contiguousBinCounts_.AddEntriesFrom(ref input, _repeated_contiguousBinCounts_codec);
            break;
          }
          case 24: {
            ContiguousBinIndexOffset = input.ReadSInt32();
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
