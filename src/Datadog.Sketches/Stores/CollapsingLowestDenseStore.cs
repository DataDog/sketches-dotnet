// <copyright file="CollapsingLowestDenseStore.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

namespace Datadog.Sketches.Stores;

/// <summary>
/// CollapsingLowestDenseStore is a dynamically growing contiguous (non-sparse) store.
/// The lower bins get combined so that the total number of bins do not exceed maxNumBins.
/// </summary>
public class CollapsingLowestDenseStore : CollapsingDenseStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollapsingLowestDenseStore"/> class.
    /// </summary>
    /// <param name="maxNumBins">Maximum number of bins</param>
    public CollapsingLowestDenseStore(int maxNumBins)
        : base(maxNumBins)
    {
    }

    private CollapsingLowestDenseStore(CollapsingLowestDenseStore store)
        : base(store)
    {
    }

    /// <inheritdoc />
    public override Store Copy() => new CollapsingLowestDenseStore(this);

    /// <inheritdoc />
    public override void MergeWith(Store store)
    {
        if (store is CollapsingLowestDenseStore collapsingDenseStore)
        {
            MergeWith(collapsingDenseStore);
        }
        else
        {
            foreach (var bin in EnumerateDescending())
            {
                Add(bin);
            }
        }
    }

    /// <inheritdoc />
    protected override int Normalize(int index)
    {
        if (index < MinIndex)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement - ExtendRange can change the value of IsCollapsed
            if (!IsCollapsed)
            {
                ExtendRange(index);
            }

            if (IsCollapsed)
            {
                return 0;
            }
        }
        else if (index > MaxIndex)
        {
            ExtendRange(index);
        }

        return index - Offset;
    }

    /// <inheritdoc />
    protected override void Adjust(int newMinIndex, int newMaxIndex)
    {
        // The cast to long is required to deal with overflows
        if ((long)newMaxIndex - newMinIndex + 1 > Counts.Length)
        {
            // The range of indices is too wide, buckets of lowest indices need to be collapsed
            newMinIndex = newMaxIndex - Counts.Length + 1;

            if (newMinIndex >= MaxIndex)
            {
                // There will be only one non-empty bucket
                var totalCount = GetTotalCount();
                ResetCounts();
                Offset = newMinIndex;
                MinIndex = newMinIndex;
                Counts[0] = totalCount;
            }
            else
            {
                var shift = Offset - newMinIndex;

                if (shift < 0)
                {
                    // Collapse the buckets
                    var collapsedCount = GetTotalCount(MinIndex, newMinIndex - 1);
                    ResetCounts(MinIndex, newMinIndex - 1);
                    Counts[newMinIndex - Offset] += collapsedCount;
                    MinIndex = newMinIndex;

                    // Shift the buckets to make room for newMaxIndex
                    ShiftCounts(shift);
                }
                else
                {
                    // Shift the buckets to make room for newMinIndex
                    ShiftCounts(shift);
                    MinIndex = newMinIndex;
                }
            }

            MaxIndex = newMaxIndex;
            IsCollapsed = true;
        }
        else
        {
            CenterCounts(newMinIndex, newMaxIndex);
        }
    }

    private void MergeWith(CollapsingLowestDenseStore store)
    {
        if (store.IsEmpty())
        {
            return;
        }

        if (store.MinIndex < MinIndex || store.MaxIndex > MaxIndex)
        {
            ExtendRange(store.MinIndex, store.MaxIndex);
        }

        var index = store.MinIndex;

        for (; index < MinIndex && index <= store.MaxIndex; index++)
        {
            Counts[0] += store.Counts[index - store.Offset];
        }

        for (; index < store.MaxIndex; index++)
        {
            Counts[index - Offset] += store.Counts[index - store.Offset];
        }

        // This is a separate test so that the comparison in the previous loop is strict (<) and handles
        // store.MaxIndex = int.MaxValue.
        if (index == store.MaxIndex)
        {
            Counts[index - Offset] += store.Counts[index - store.Offset];
        }
    }
}
