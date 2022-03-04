// <copyright file="CollapsingHighestDenseStoreTests.cs" company="Datadog">
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
public class CollapsingHighestDenseStoreTests : StoreTests
{
    private readonly int _maxNumBins;

    public CollapsingHighestDenseStoreTests(int maxNumBins)
    {
        _maxNumBins = maxNumBins;
    }

    protected override Store NewStore() => new CollapsingHighestDenseStore(_maxNumBins);

    protected override IDictionary<int, double> GetCounts(Bin[] bins)
    {
        var nonEmptyBins = bins.Where(b => b.Count > 0).ToArray();

        if (nonEmptyBins.Length == 0)
        {
            return new Dictionary<int, double>();
        }

        var minIndex = nonEmptyBins.Min(b => b.Index);
        var maxStorableIndex = (int)Math.Min(int.MaxValue, (long)minIndex + _maxNumBins - 1);

        return bins.GroupBy(b => Math.Min(b.Index, maxStorableIndex))
            .ToDictionary(g => g.Key, g => g.Sum(b => b.Count));
    }
}
