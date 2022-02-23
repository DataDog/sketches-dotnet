// <copyright file="CollapsingHighestDenseStore.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

namespace Datadog.Sketches.Stores
{
    /// <summary>
    /// CollapsingHighestDenseStore is a dynamically growing contiguous (non-sparse) store.
    /// The highest bins get combined so that the total number of bins do not exceed maxNumBins.
    /// </summary>
    public class CollapsingHighestDenseStore : CollapsingDenseStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollapsingHighestDenseStore"/> class.
        /// </summary>
        /// <param name="maxNumBins">Maximum number of bins</param>
        public CollapsingHighestDenseStore(int maxNumBins)
            : base(maxNumBins)
        {
        }

        private CollapsingHighestDenseStore(CollapsingHighestDenseStore store)
            : base(store)
        {
        }

        /// <inheritdoc />
        public override void MergeWith(Store store)
        {
            if (store is CollapsingHighestDenseStore collapsingDenseStore)
            {
                MergeWith(collapsingDenseStore);
            }
            else
            {
                foreach (var bin in EnumerateAscending())
                {
                    Add(bin);
                }
            }
        }

        /// <inheritdoc />
        public override Store Copy() => new CollapsingHighestDenseStore(this);

        /// <inheritdoc />
        protected override int Normalize(int index)
        {
            if (index > MaxIndex)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement - ExtendRange can change the value of IsCollapsed
                if (!IsCollapsed)
                {
                    ExtendRange(index);
                }

                if (IsCollapsed)
                {
                    return Counts.Length - 1;
                }
            }
            else if (index < MinIndex)
            {
                ExtendRange(index);
            }

            return index - Offset;
        }

        /// <inheritdoc />
        protected override void Adjust(int newMinIndex, int newMaxIndex)
        {
            if (newMaxIndex - newMinIndex + 1 > Counts.Length)
            {
                // The range of indices is too wide, buckets of lowest indices need to be collapsed
                newMaxIndex = newMinIndex + Counts.Length - 1;

                if (newMaxIndex <= MinIndex)
                {
                    // There will be only one non-empty bucket
                    var totalCount = GetTotalCount();
                    ResetCounts();
                    Offset = newMinIndex;
                    MaxIndex = newMaxIndex;
                    Counts[Counts.Length - 1] = totalCount;
                }
                else
                {
                    var shift = Offset - newMinIndex;

                    if (shift > 0)
                    {
                        // Collapse the buckets
                        var collapsedCount = GetTotalCount(newMaxIndex + 1, MaxIndex);
                        ResetCounts(newMaxIndex + 1, MaxIndex);
                        Counts[newMaxIndex - Offset] += collapsedCount;
                        MaxIndex = newMaxIndex;

                        // Shift the buckets to make room for newMinIndex
                        ShiftCounts(shift);
                    }
                    else
                    {
                        ShiftCounts(shift);
                        MaxIndex = newMaxIndex;
                    }
                }
            }
        }

        private void MergeWith(CollapsingHighestDenseStore store)
        {
            if (store.IsEmpty())
            {
                return;
            }

            if (store.MinIndex < MinIndex || store.MaxIndex > MaxIndex)
            {
                ExtendRange(store.MinIndex, store.MaxIndex);
            }

            var index = store.MaxIndex;

            for (; index > MaxIndex && index >= store.MinIndex; index--)
            {
                Counts[Counts.Length - 1] += store.Counts[index - store.Offset];
            }

            for (; index > store.MinIndex; index--)
            {
                Counts[index - Offset] += store.Counts[index - store.Offset];
            }

            // This is a separate test so that the comparison in the previous loop is strict (>) and handles
            // store.MinIndex = int.MinValue.

            if (index == store.MinIndex)
            {
                Counts[index - Offset] += store.Counts[index - store.Offset];
            }
        }
    }
}
