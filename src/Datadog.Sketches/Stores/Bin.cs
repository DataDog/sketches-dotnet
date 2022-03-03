// <copyright file="Bin.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Diagnostics;

namespace Datadog.Sketches.Stores
{
    /// <summary>
    /// A pair of index and count.
    /// </summary>
    [DebuggerDisplay("({Index}, {Count})")]
    public readonly struct Bin : IEquatable<Bin>
    {
        /// <summary>
        /// The index of the bin
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// The count of the bin
        /// </summary>
        public readonly double Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bin"/> struct.
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="count">The count</param>
        public Bin(int index, double count)
        {
            Index = index;
            Count = count;
        }

        /// <inheritdoc />
        public bool Equals(Bin other)
        {
            return Index == other.Index && Count.Equals(other.Count);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Bin other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Index * 397) ^ Count.GetHashCode();
            }
        }
    }
}
