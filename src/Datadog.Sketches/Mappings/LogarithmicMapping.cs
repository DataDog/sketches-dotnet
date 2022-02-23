// <copyright file="LogarithmicMapping.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

#pragma warning disable SA1600 // Elements should be documented

using System;

namespace Datadog.Sketches.Mappings
{
    /// <summary>
    /// LogarithmicMapping is an IndexMapping that is memory-optimal,
    /// that is to say that given a targeted relative accuracy, it
    /// requires the least number of indices to cover a given range of values.
    /// This is done by logarithmically mapping floating-point values to integers.
    /// </summary>
    public class LogarithmicMapping : IIndexMapping
    {
        private const double MinNormal = 2.2250738585072014E-308;
        private readonly double _indexOffset;
        private readonly double _multiplier;

        public LogarithmicMapping(double relativeAccuracy)
            : this((1 + relativeAccuracy) / (1 - relativeAccuracy), 0)
        {
        }

        public LogarithmicMapping(double gamma, double indexOffset)
        {
            if (gamma <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(gamma), "Gamma must be greater than 1");
            }

            _indexOffset = indexOffset;
            _multiplier = 1 / Math.Log(gamma);
            RelativeAccuracy = 1 - (2 / (1 + gamma));

            unchecked
            {
                MinIndexableValue = Math.Max(
                    Math.Exp(((int.MinValue - indexOffset) / _multiplier) + 1),
                    MinNormal * gamma);

                MaxIndexableValue = Math.Min(
                    Math.Exp(((int.MaxValue - indexOffset) / _multiplier) - 1),
                    Math.Exp(double.MaxValue / (2 * gamma) * (gamma + 1)));
            }
        }

        /// <inheritdoc />
        public double RelativeAccuracy { get; }

        /// <inheritdoc />
        public double MinIndexableValue { get; }

        /// <inheritdoc />
        public double MaxIndexableValue { get; }

        /// <inheritdoc />
        public int GetIndex(double value)
        {
            var index = (Math.Log(value) * _multiplier) + _indexOffset;
            return index >= 0 ? (int)index : (int)index - 1;
        }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            return GetLowerBound(index) * (1 + RelativeAccuracy);
        }

        /// <inheritdoc />
        public double GetLowerBound(int index)
        {
            return Math.Exp((index - _indexOffset) / _multiplier);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            var other = (LogarithmicMapping)obj;

            return _indexOffset.Equals(other._indexOffset) && _multiplier.Equals(other._multiplier);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (_indexOffset.GetHashCode() * 397) ^ _multiplier.GetHashCode();
            }
        }
    }
}
