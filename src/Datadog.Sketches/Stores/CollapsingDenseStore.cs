// <copyright file="CollapsingDenseStore.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

#pragma warning disable SA1600 // Elements should be documented

using System;

namespace Datadog.Sketches.Stores
{
    public abstract class CollapsingDenseStore : DenseStore
    {
        private readonly int _maxNumBins;

        protected CollapsingDenseStore(int maxNumBins)
        {
            _maxNumBins = maxNumBins;
            IsCollapsed = false;
        }

        protected CollapsingDenseStore(CollapsingDenseStore store)
            : base(store)
        {
            _maxNumBins = store._maxNumBins;
            IsCollapsed = store.IsCollapsed;
        }

        protected bool IsCollapsed { get; set; }

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();
            IsCollapsed = false;
        }

        /// <inheritdoc />
        protected override long GetNewLength(int newMinIndex, int newMaxIndex)
        {
            return Math.Min(base.GetNewLength(newMinIndex, newMaxIndex), _maxNumBins);
        }
    }
}
