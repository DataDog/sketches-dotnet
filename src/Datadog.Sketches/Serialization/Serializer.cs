// <copyright file="Serializer.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using System.Text;

namespace Datadog.Sketches.Serialization
{
    /// <summary>
    /// This class is used to perform protobuf serialization compliant with the official schema used to
    /// generate protobuf bindings (DDSketch.proto) but does not require to reference an external library.
    /// </summary>
    internal class Serializer : IDisposable
    {
        /// <summary>
        /// Any integer type including booleans
        /// </summary>
        private const int VarInt = 0;

        /// <summary>
        /// Doubles
        /// </summary>
        private const int Fixed64 = 1;

        /// <summary>
        /// Strings, binary, arrays (i.e. repeated fields), embedded structs (i.e. messages)
        /// </summary>
        private const int LengthDelimited = 2;

        private static readonly int[] VarIntLengths32;

        private readonly BinaryWriter _writer;

        static Serializer()
        {
            VarIntLengths32 = new int[33];

            for (var i = 0; i < VarIntLengths32.Length; i++)
            {
                VarIntLengths32[i] = (31 - i) / 7;
            }
        }

        public Serializer(Stream output)
        {
            _writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
        }

        public static int EmbeddedSize(int size)
        {
            return VarIntLength(size) + 1 + size;
        }

        public static int FieldSize(int fieldIndex, int value)
        {
            return value == 0 ? 0 : (TagSize(fieldIndex, VarInt) + VarIntLength(value) + 1);
        }

        public static int EmbeddedFieldSize(int fieldIndex, int size)
        {
            return TagSize(fieldIndex, LengthDelimited) + EmbeddedSize(size);
        }

        public static int DoubleFieldSize(int fieldIndex, double value)
        {
            return value == 0D ? 0 : (TagSize(fieldIndex, Fixed64) + sizeof(double));
        }

        public static int SignedIntFieldSize(int fieldIndex, int value)
        {
            return value == 0 ? 0 : TagSize(fieldIndex, VarInt) + VarIntLength(ZigZag(value)) + 1;
        }

        public static int CompactDoubleArraySize(int fieldIndex, int size)
        {
            return TagSize(fieldIndex, LengthDelimited) + EmbeddedSize(size * sizeof(double));
        }

        public static int BinSize(int fieldPosition, int index, double count)
        {
            return EmbeddedFieldSize(fieldPosition, SignedIntFieldSize(1, index) + DoubleFieldSize(2, count));
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void WriteHeader(int fieldNumber, int length)
        {
            WriteTag(fieldNumber, LengthDelimited);
            WriteVarInt(length);
        }

        public void WriteCompactArray(int fieldIndex, double[] array, int from, int length)
        {
            WriteTag(fieldIndex, LengthDelimited);
            WriteVarInt(length * sizeof(double));

            for (var i = from; i < from + length; i++)
            {
                WriteDoubleLittleEndian(array[i]);
            }
        }

        public void WriteDouble(int fieldIndex, double value)
        {
            if (value != 0)
            {
                WriteTag(fieldIndex, Fixed64);
                WriteDoubleLittleEndian(value);
            }
        }

        public void WriteUnsignedInt32(int fieldIndex, int value)
        {
            if (value != 0)
            {
                WriteTag(fieldIndex, VarInt);
                WriteVarInt(value);
            }
        }

        public void WriteSignedInt32(int fieldIndex, int value)
        {
            if (value != 0)
            {
                WriteTag(fieldIndex, VarInt);
                WriteVarInt(ZigZag(value));
            }
        }

        public void WriteBin(int fieldPosition, int index, double count)
        {
            var length = SignedIntFieldSize(1, index) + DoubleFieldSize(2, count);
            WriteHeader(fieldPosition, length);
            WriteSignedInt32(1, index);
            WriteDouble(2, count);
        }

        private static int TagSize(int tag, int type)
        {
            unchecked
            {
                return VarIntLength((tag << 3) | type) + 1;
            }
        }

        private static int ZigZag(int signed) => (signed << 1) ^ (signed >> 31);

        private static int VarIntLength(int value) => VarIntLengths32[NumberOfLeadingZeros(unchecked((uint)value))];

        /// <summary>
        /// C# implementation of Java Integer.numberOfLeadingZeros
        /// </summary>
        private static uint NumberOfLeadingZeros(uint x)
        {
#if NETCOREAPP3_1_OR_GREATER
            return (uint)System.Numerics.BitOperations.LeadingZeroCount(x);
#else
            // https://stackoverflow.com/a/10439333/869621
            const int numIntBits = sizeof(int) * 8; // compile time constant

            // Do the smearing
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;

            // Count the ones
            x -= x >> 1 & 0x55555555;
            x = (x >> 2 & 0x33333333) + (x & 0x33333333);
            x = (x >> 4) + x & 0x0f0f0f0f;
            x += x >> 8;
            x += x >> 16;

            return numIntBits - (x & 0x0000003f); // subtract # of 1s from 32
#endif
        }

        private void WriteDoubleLittleEndian(double value)
        {
#if NETFRAMEWORK
            // .NET Framework is only supported on little-endian architectures
            _writer.Write(value);
#else
            if (!BitConverter.IsLittleEndian)
            {
                var tmp = System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value));
                _writer.Write(tmp);
            }
            else
            {
                _writer.Write(value);
            }
#endif
        }

        private void WriteTag(int fieldIndex, int wireType)
        {
            WriteVarInt((fieldIndex << 3) | wireType);
        }

        private void WriteVarInt(int value)
        {
            var length = VarIntLength(value);

            for (var i = 0; i < length; i++)
            {
                _writer.Write((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }

            _writer.Write((byte)value);
        }
    }
}