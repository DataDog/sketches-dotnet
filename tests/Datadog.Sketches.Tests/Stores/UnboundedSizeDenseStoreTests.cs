// <copyright file="UnboundedSizeDenseStoreTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Datadog.Sketches.Stores;

namespace Datadog.Sketches.Tests.Stores;

public class UnboundedSizeDenseStoreTests : StoreTests
{
    public override void TestExtremeValues()
    {
        // UnboundedSizeDenseStore is not meant to be used with values that are extremely far from one
        // another as it would allocate an excessively large array.
    }

    public override void TestMergingExtremeValues()
    {
        // UnboundedSizeDenseStore is not meant to be used with values that are extremely far from one
        // another as it would allocate an excessively large array.
    }

    protected override Store NewStore() => new UnboundedSizeDenseStore();

    protected override IDictionary<int, double> GetCounts(Bin[] bins)
    {
        return bins.GroupBy(b => b.Index)
            .ToDictionary(g => g.Key, g => g.Sum(b => b.Count));
    }
}
