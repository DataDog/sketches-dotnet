// <copyright file="CollapsingLowestDenseStoreTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Datadog.Sketches.Stores;
using NUnit.Framework;

namespace Datadog.Sketches.Tests.Stores;

[TestFixture(1)]
[TestFixture(20)]
[TestFixture(1000)]
public class CollapsingLowestDenseStoreTests : StoreTests
{
    private readonly int _maxBins;

    public CollapsingLowestDenseStoreTests(int maxBins)
    {
        _maxBins = maxBins;
    }

    protected override Store NewStore() => new CollapsingLowestDenseStore(_maxBins);

    protected override IDictionary<int, double> GetCounts(Bin[] bins)
    {
        var nonEmptyBins = bins.Where(b => b.Count > 0).ToArray();

        if (nonEmptyBins.Length == 0)
        {
            return new Dictionary<int, double>();
        }

        var maxIndex = nonEmptyBins.Max(b => b.Index);
        var minStorableIndex = (int)Math.Max(int.MinValue, (long)maxIndex - _maxBins + 1);

        return bins.GroupBy(b => Math.Max(b.Index, minStorableIndex))
            .ToDictionary(g => g.Key, g => g.Sum(b => b.Count));
    }
}
