// <copyright file="Store.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Datadog.Sketches.Stores;

/// <summary>
/// An object that maps integers to counters. It can be seen as a collection of Bin, which are pairs of indices and counters.
/// </summary>
public abstract class Store : IEnumerable<Bin>
{
    /// <summary>
    /// Increments the counter at the specified index.
    /// </summary>
    /// <param name="index">The index of the counter to be updated.</param>
    public virtual void Add(int index)
    {
        Add(index, 1);
    }

    /// <summary>
    /// Updates the counter at the specified index.
    /// </summary>
    /// <param name="index">The index of the counter to be updated.</param>
    /// <param name="count">A non-negative value</param>
    public virtual void Add(int index, long count)
    {
        Add(index, (double)count);
    }

    /// <summary>
    /// Updates the counter at the specified index.
    /// </summary>
    /// <param name="bin">The bin to be used for updating the counter.</param>
    public virtual void Add(Bin bin)
    {
        Add(bin.Index, bin.Count);
    }

    /// <summary>
    /// Merges another store into this one.
    /// </summary>
    /// <param name="store">The store to be merged into this one.</param>
    public virtual void MergeWith(Store store)
    {
        foreach (var bin in store)
        {
            Add(bin);
        }
    }

    /// <summary>
    /// Updates the counter at the specified index.
    /// </summary>
    /// <param name="index">The index of the counter to be updated.</param>
    /// <param name="count">A non-negative value</param>
    public abstract void Add(int index, double count);

    /// <summary>
    /// Returns a deep copy of this store.
    /// </summary>
    /// <returns>Copy of the store.</returns>
    public abstract Store Copy();

    /// <summary>
    /// Zeroes all counts in the store.
    /// The store behaves as if empty after this call, but no underlying storage is released.
    /// </summary>
    public abstract void Clear();

    /// <summary>
    /// Check if the store contains any non-zero counter.
    /// </summary>
    /// <returns>Returns true if the store does not contain any non-zero counter, false otherwise.</returns>
    public virtual bool IsEmpty()
    {
        foreach (var bin in this)
        {
            if (bin.Count != 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the index of the lowest non-zero counter.
    /// </summary>
    /// <returns>The index of the lowest non-zero counter.</returns>
    public abstract int GetMinIndex();

    /// <summary>
    /// Gets the index of the highest non-zero counter.
    /// </summary>
    /// <returns>The index of the highest non-zero counter.</returns>
    public abstract int GetMaxIndex();

    /// <summary>
    /// Gets the sum of the counters of this store.
    /// </summary>
    /// <returns>The sum of the counters of this store.</returns>
    public abstract double GetTotalCount();

    /// <inheritdoc />
    public abstract IEnumerator<Bin> GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Enumerates the store in ascending order
    /// </summary>
    /// <returns>Enumerable</returns>
    public virtual IEnumerable<Bin> EnumerateAscending() => this;

    /// <summary>
    /// Enumerates the store in descending order
    /// </summary>
    /// <returns>Enumerable</returns>
    public virtual IEnumerable<Bin> EnumerateDescending() => this.Reverse();
}
