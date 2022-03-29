// <copyright file="DDSketch.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datadog.Sketches.Mappings;
using Datadog.Sketches.Serialization;
using Datadog.Sketches.Stores;

namespace Datadog.Sketches;

/// <summary>
/// A quantile sketch with relative-error guarantees. This sketch computes
/// quantile values with an approximation error that is relative to the actual
/// quantile value. It works on both negative and non-negative input values.
/// For instance, using DDSketch with a relative accuracy guarantee set to 1%, if
/// the expected quantile value is 100, the computed quantile value is guaranteed to
/// be between 99 and 101. If the expected quantile value is 1000, the computed
/// quantile value is guaranteed to be between 990 and 1010.
/// DDSketch works by mapping floating-point input values to bins and counting the
/// number of values for each bin. The underlying structure that keeps track of bin
/// counts is store.
/// The memory size of the sketch depends on the range that is covered by the input
/// values: the larger that range, the more bins are needed to keep track of the
/// input values. As a rough estimate, if working on durations with a relative
/// accuracy of 2%, about 2kB (275 bins) are needed to cover values between 1
/// millisecond and 1 minute, and about 6kB (802 bins) to cover values between 1
/// nanosecond and 1 day.
/// The size of the sketch can be have a fail-safe upper-bound by using collapsing
/// stores. As shown in
/// <see href="http://www.vldb.org/pvldb/vol12/p2195-masson.pdf" />;
/// the likelihood of a store collapsing when using the default bound is vanishingly
/// small for most data.
/// </summary>
public class DDSketch
{
    private readonly double _minIndexedValue;
    private readonly double _maxIndexedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DDSketch"/> class.
    /// </summary>
    /// <param name="indexMapping">Map between values and store bins</param>
    /// <param name="negativeValueStore">Storage for negative values</param>
    /// <param name="positiveValueStore">Storage for positive values</param>
    public DDSketch(IIndexMapping indexMapping, Store negativeValueStore, Store positiveValueStore)
        : this(indexMapping, negativeValueStore, positiveValueStore, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DDSketch"/> class.
    /// </summary>
    /// <param name="indexMapping">Map between values and store bins</param>
    /// <param name="negativeValueStore">Storage for negative values</param>
    /// <param name="positiveValueStore">Storage for positive values</param>
    /// <param name="minIndexedValue">The minimum value seen by the sketch</param>
    public DDSketch(IIndexMapping indexMapping, Store negativeValueStore, Store positiveValueStore, double minIndexedValue)
        : this(indexMapping, negativeValueStore, positiveValueStore, minIndexedValue, 0)
    {
    }

    internal DDSketch(IIndexMapping indexMapping, Store negativeValueStore, Store positiveValueStore, double minIndexedValue, double zeroCount)
    {
        IndexMapping = indexMapping;
        NegativeValueStore = negativeValueStore;
        PositiveValueStore = positiveValueStore;
        _minIndexedValue = Math.Max(minIndexedValue, indexMapping.MinIndexableValue);
        _maxIndexedValue = indexMapping.MaxIndexableValue;
        ZeroCount = zeroCount;
    }

    /// <summary>
    /// Gets the map between values and store bins.
    /// </summary>
    public IIndexMapping IndexMapping { get; }

    /// <summary>
    /// Gets the number of zeros stored in the sketch.
    /// </summary>
    public double ZeroCount { get; private set; }

    /// <summary>
    /// Gets the positive values stored in the sketch.
    /// </summary>
    public Store PositiveValueStore { get; }

    /// <summary>
    /// Gets the negative values stored in the sketch.
    /// </summary>
    public Store NegativeValueStore { get; }

    /// <summary>
    /// Adds a value to the sketch.
    /// </summary>
    /// <param name="value">Value to be added</param>
    public void Add(double value)
    {
        Add(value, 1);
    }

    /// <summary>
    /// Adds a value to the sketch with a floating-point count.
    /// If count is an integer, calling <code>Add(value, count)</code> is equivalent to calling <code>Add(value)</code> count times.
    /// </summary>
    /// <param name="value">The value to be added</param>
    /// <param name="count">The weight associated with the value to be added</param>
    public void Add(double value, double count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "The count cannot be negative");
        }

        if (value > _minIndexedValue)
        {
            if (value > _maxIndexedValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The input value is outside the range that is tracked by the sketch (too high)");
            }

            PositiveValueStore.Add(IndexMapping.GetIndex(value), count);
        }
        else if (value < -_minIndexedValue)
        {
            if (value < -_maxIndexedValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The input value is outside the range that is tracked by the sketch (too low)");
            }

            NegativeValueStore.Add(IndexMapping.GetIndex(-value), count);
        }
        else if (double.IsNaN(value))
        {
            throw new ArgumentException("Value is not a number", nameof(value));
        }
        else
        {
            ZeroCount += count;
        }
    }

    /// <summary>
    /// Sets all counts in the sketch to zero. The sketch behaves as if it were empty after this call,
    /// but any allocated memory is not released.
    /// </summary>
    public void Clear()
    {
        PositiveValueStore.Clear();
        NegativeValueStore.Clear();
        ZeroCount = 0;
    }

    /// <summary>
    /// Gets the total number of values that have been added to this sketch.
    /// </summary>
    /// <returns>The total number of values</returns>
    public double GetCount()
    {
        return ZeroCount + PositiveValueStore.GetTotalCount() + NegativeValueStore.GetTotalCount();
    }

    /// <summary>
    /// Gets the value at the specified quantile.
    /// </summary>
    /// <param name="quantile">A number between 0 and 1 (inclusive)</param>
    /// <returns>The value of the specified quantile</returns>
    public double GetValueAtQuantile(double quantile) => GetValueAtQuantile(quantile, GetCount());

    /// <summary>
    /// Gets the values at the respective specified quantiles.
    /// </summary>
    /// <param name="quantiles">Numbers between 0 and 1 (inclusive)</param>
    /// <returns>The values at the respective specified quantiles</returns>
    public IEnumerable<double> GetValuesAtQuantiles(IEnumerable<double> quantiles)
    {
        var count = GetCount();

        if (count == 0)
        {
            throw new InvalidOperationException("The sketch is empty");
        }

        return quantiles.Select(q => GetValueAtQuantile(q, count));
    }

    /// <summary>
    /// Merges the other sketch into this one. After this operation, this sketch encodes the values
    /// that were added to both this and the other sketch.
    /// </summary>
    /// <param name="other">The sketch to be merged into this one</param>
    public void MergeWith(DDSketch other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (!IndexMapping.Equals(other.IndexMapping))
        {
            throw new InvalidOperationException("The sketches are not mergeable because they do not use the same index mappings.");
        }

        NegativeValueStore.MergeWith(other.NegativeValueStore);
        PositiveValueStore.MergeWith(other.PositiveValueStore);
        ZeroCount += other.ZeroCount;
    }

    /// <summary>
    /// Checks if the sketch is empty.
    /// </summary>
    /// <returns>True if no value has been added to this sketch, false otherwise</returns>
    public bool IsEmpty()
    {
        return ZeroCount == 0 && PositiveValueStore.IsEmpty() && NegativeValueStore.IsEmpty();
    }

    /// <summary>
    /// Gets the maximum value that has been added to this sketch.
    /// </summary>
    /// <returns>Maximum value</returns>
    public double GetMaxValue()
    {
        if (!PositiveValueStore.IsEmpty())
        {
            return IndexMapping.GetValue(PositiveValueStore.GetMaxIndex());
        }

        if (ZeroCount > 0)
        {
            return 0;
        }

        return -IndexMapping.GetValue(NegativeValueStore.GetMinIndex());
    }

    /// <summary>
    /// Gets the minimum value that has been added to this sketch.
    /// </summary>
    /// <returns>Minimum value</returns>
    public double GetMinValue()
    {
        if (!NegativeValueStore.IsEmpty())
        {
            return -IndexMapping.GetValue(NegativeValueStore.GetMaxIndex());
        }

        if (ZeroCount > 0)
        {
            return 0;
        }

        return IndexMapping.GetValue(PositiveValueStore.GetMinIndex());
    }

    /// <summary>
    /// Gets an approximation of the sum of the values that have been added to the sketch. If the
    /// values that have been added to the sketch all have the same sign, the approximation error has
    /// the relative accuracy guarantees of the <see cref="IIndexMapping"/> used for this sketch.
    /// </summary>
    /// <returns>An approximation of the sum of the values that have been added to the sketch</returns>
    public double GetSum()
    {
        double sum = 0;

        foreach (var bin in NegativeValueStore)
        {
            sum -= IndexMapping.GetValue(bin.Index) * bin.Count;
        }

        foreach (var bin in PositiveValueStore)
        {
            sum += IndexMapping.GetValue(bin.Index) * bin.Count;
        }

        return sum;
    }

    internal int ComputeSerializedSize()
    {
        return Serializer.EmbeddedFieldSize(1, ((ISerializable)IndexMapping).ComputeSerializedSize())
            + Serializer.EmbeddedFieldSize(2, ((ISerializable)PositiveValueStore).ComputeSerializedSize())
            + Serializer.EmbeddedFieldSize(3, ((ISerializable)NegativeValueStore).ComputeSerializedSize())
            + Serializer.DoubleFieldSize(4, ZeroCount);
    }

    /// <summary>
    /// Produces protobuf encoded bytes which are equivalent to using the official protobuf bindings,
    /// without requiring a runtime dependency on a protobuf library.
    /// </summary>
    /// <param name="output">The output stream for the serialized sketch</param>
    internal void Serialize(Stream output)
    {
        using var serializer = new Serializer(output);

        var serializableIndexMapping = (ISerializable)IndexMapping;

        serializer.WriteHeader(1, serializableIndexMapping.ComputeSerializedSize());
        serializableIndexMapping.Serialize(serializer);

        var serializablePositiveValueStore = (ISerializable)PositiveValueStore;

        serializer.WriteHeader(2, serializablePositiveValueStore.ComputeSerializedSize());
        serializablePositiveValueStore.Serialize(serializer);

        var serializableNegativeValueStore = (ISerializable)NegativeValueStore;

        serializer.WriteHeader(3, serializableNegativeValueStore.ComputeSerializedSize());
        serializableNegativeValueStore.Serialize(serializer);

        serializer.WriteDouble(4, ZeroCount);
    }

    private double GetValueAtQuantile(double quantile, double count)
    {
        if (quantile < 0 || quantile > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantile), "The quantile must be between 0 and 1");
        }

        if (count == 0)
        {
            throw new InvalidOperationException("The sketch is empty");
        }

        var rank = quantile * (count - 1);

        double n = 0;

        foreach (var bin in NegativeValueStore.EnumerateDescending())
        {
            if ((n += bin.Count) > rank)
            {
                return -IndexMapping.GetValue(bin.Index);
            }
        }

        if ((n += ZeroCount) > rank)
        {
            return 0;
        }

        foreach (var bin in PositiveValueStore.EnumerateAscending())
        {
            if ((n += bin.Count) > rank)
            {
                return IndexMapping.GetValue(bin.Index);
            }
        }

        throw new InvalidOperationException("Element not found");
    }
}
