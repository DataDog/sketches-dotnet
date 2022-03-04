// <copyright file="StoreTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Datadog.Sketches.Stores;
using FluentAssertions;
using NUnit.Framework;

namespace Datadog.Sketches.Tests.Stores;

public abstract class StoreTests
{
    [Test]
    public void TestEmpty()
    {
        TestAdding();
    }

    [Test]
    public void TestConstant()
    {
        TestAdding(Enumerable.Repeat(0, 10_000).ToArray());
    }

    [Test]
    public void TestIncreasingLinearly()
    {
        TestAdding(Enumerable.Range(0, 10_000).ToArray());
    }

    [Test]
    public void TestClear()
    {
        var store = NewStore();
        store.IsEmpty().Should().BeTrue();

        store.Clear();
        store.IsEmpty().Should().BeTrue();

        var beforeClear = Enumerable.Range(0, 10_000).ToArray();

        foreach (var value in beforeClear)
        {
            store.Add(value);
        }

        Test(ToBins(beforeClear), store);
        store.IsEmpty().Should().BeFalse();

        store.Clear();
        store.IsEmpty().Should().BeTrue();

        var afterClear = Enumerable.Range(10_000, 10_000).ToArray();

        foreach (var value in afterClear)
        {
            store.Add(value);
        }

        Test(ToBins(afterClear), store);
    }

    [Test]
    public void TestDecreasingLinearly()
    {
        TestAdding(Enumerable.Range(0, 10_000).Select(i => -i).ToArray());
    }

    [Test]
    public void TestIncreasingExponentially()
    {
        TestAdding(Enumerable.Range(0, 16).Select(i => (int)Math.Pow(2, i)).ToArray());
    }

    [Test]
    public void TestDecreasingExponentially()
    {
        TestAdding(Enumerable.Range(0, 16).Select(i => -(int)Math.Pow(2, i)).ToArray());
    }

    [Test]
    public void TestBinCounts()
    {
        TestAdding(Enumerable.Range(0, 10).Select(i => new Bin(i, 2 * i)).ToArray());
        TestAdding(Enumerable.Range(0, 10).Select(i => new Bin(-i, 2 * i)).ToArray());
    }

    [Test]
    public void TestNonIntegerCounts()
    {
        TestAdding(Enumerable.Range(0, 10).Select(i => new Bin(i, Math.Log(i + 1))).ToArray());
        TestAdding(Enumerable.Range(0, 10).Select(i => new Bin(-i, Math.Log(i + 1))).ToArray());
    }

    [Test]
    public virtual void TestExtremeValues()
    {
        TestAdding(int.MinValue);
        TestAdding(int.MaxValue);
        TestAdding(0, int.MinValue);
        TestAdding(0, int.MaxValue);
        TestAdding(int.MinValue, int.MaxValue);
        TestAdding(int.MaxValue, int.MinValue);
    }

    [Test]
    public void TestMergingEmpty()
    {
        TestMerging(Array.Empty<int>(), Array.Empty<int>());
        TestMerging(Array.Empty<int>(), new[] { 0 });
        TestMerging(new[] { 0 }, Array.Empty<int>());
    }

    [Test]
    public void TestMergingFarApart()
    {
        TestMerging(new[] { -10_000 }, new[] { 10_000 });
        TestMerging(new[] { 10_000 }, new[] { -10_000 });
        TestMerging(new[] { 10_000 }, new[] { -10_000 }, new[] { 0 });
        TestMerging(new[] { -10_000, 10_000 }, new[] { -5_000, 5_000 });
        TestMerging(new[] { -5_000, 5_000 }, new[] { -10_000, 10_000 });
        TestMerging(new[] { -5_000, 10_000 }, new[] { -10_000, 5_000 });
        TestMerging(new[] { 10_000, 0 }, new[] { -10_000 }, new[] { 0 });
    }

    [Test]
    public void TestMergingConstant()
    {
        TestMerging(new[] { 2, 2 }, new[] { 2, 2, 2 }, new[] { 2 });
        TestMerging(new[] { -8, -8 }, Array.Empty<int>(), new[] { -8 });
    }

    [Test]
    public void TestMergingNonIntegerCounts()
    {
        TestMerging(new[] { new Bin(3, Math.PI) }, new[] { new Bin(3, Math.E) });
        TestMerging(
            new[] { new Bin(0, 0.1), new Bin(2, 0.3) },
            new[] { new Bin(-1, 0.9), new Bin(0, 0.7), new Bin(2, 0.1) });
    }

    [Test]
    public virtual void TestMergingExtremeValues()
    {
        TestMerging(new[] { 0 }, new[] { int.MinValue });
        TestMerging(new[] { 0 }, new[] { int.MaxValue });
        TestMerging(new[] { int.MinValue }, new[] { 0 });
        TestMerging(new[] { int.MaxValue }, new[] { 0 });
        TestMerging(new[] { int.MinValue }, new[] { int.MinValue });
        TestMerging(new[] { int.MaxValue }, new[] { int.MaxValue });
        TestMerging(new[] { int.MinValue }, new[] { int.MaxValue });
        TestMerging(new[] { int.MaxValue }, new[] { int.MinValue });
        TestMerging(new[] { 0 }, new[] { int.MinValue, int.MinValue });
        TestMerging(new[] { int.MinValue, int.MinValue }, new[] { 0 });
    }

    [Test]
    public void TestCopyingEmpty()
    {
        var store = NewStore();
        Test(Array.Empty<Bin>(), store.Copy());
    }

    [Test]
    public void TestCopyingNonEmpty()
    {
        var bins = new[] { new Bin(0, 1) };
        var store = NewStore();

        foreach (var bin in bins)
        {
            store.Add(bin);
        }

        Test(bins, store.Copy());
    }

    [Test]
    public void TestCountsPreservedAfterCopy()
    {
        var store = NewStore();
        store.Add(10);
        store.Add(100);

        var copy = store.Copy();

        copy.GetTotalCount().Should().BeApproximately(store.GetTotalCount(), RelativeAccuracyTester.FloatingPointAcceptableError);
    }

    protected abstract Store NewStore();

    protected abstract IDictionary<int, double> GetCounts(Bin[] bins);

    private static Bin[] ToBins(int[] values) => values.Select(v => new Bin(v, 1)).ToArray();

    private static IDictionary<int, double> GetNonZeroCounts(IDictionary<int, double> counts)
    {
        return counts.Where(kvp => kvp.Value > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static void AssertEncodes(IDictionary<int, double> expectedCounts, Store store)
    {
        var expectedTotalCount = expectedCounts.Values.Sum();
        store.GetTotalCount().Should().BeApproximately(expectedTotalCount, RelativeAccuracyTester.FloatingPointAcceptableError);

        if (expectedTotalCount == 0)
        {
            store.IsEmpty().Should().BeTrue();
            store.Invoking(s => s.GetMinIndex()).Should().Throw<InvalidOperationException>();
            store.Invoking(s => s.GetMaxIndex()).Should().Throw<InvalidOperationException>();
        }
        else
        {
            store.IsEmpty().Should().BeFalse();

            var nonZeroKeys = expectedCounts
                .Where(kvp => kvp.Value != 0)
                .Select(kvp => kvp.Key);

            store.GetMinIndex().Should().Be(nonZeroKeys.Min());
            store.GetMaxIndex().Should().Be(nonZeroKeys.Max());
        }

        AssertSameCounts(expectedCounts, store);
        AssertSameCounts(expectedCounts, store.EnumerateAscending());
        AssertSameCounts(expectedCounts, store.EnumerateDescending());
    }

    private static void AssertSameCounts(IDictionary<int, double> expected, IEnumerable<Bin> actual)
    {
        var actualDictionary = actual
            .GroupBy(b => b.Index)
            .ToDictionary(g => g.Key, g => g.Sum(b => b.Count));

        // I originally wrote actualDictionary.Keys.Should().BeEquivalentTo(expected.Keys) but it takes seconds (!) to execute on large sets
        CollectionAssert.AreEquivalent(expected.Keys, actualDictionary.Keys);

        foreach (var key in expected.Keys)
        {
            actualDictionary[key].Should().BeApproximately(expected[key], RelativeAccuracyTester.FloatingPointAcceptableError);
        }
    }

    private void TestAdding(params int[] values)
    {
        var store = NewStore();

        foreach (var value in values)
        {
            store.Add(value);
        }

        Test(ToBins(values), store);
    }

    private void TestAdding(Bin[] bins)
    {
        var store1 = NewStore();
        var store2 = NewStore();

        foreach (var bin in bins)
        {
            store1.Add(bin);
            store2.Add(bin.Index, bin.Count);
        }

        Test(bins, store1);
        Test(bins, store2);
    }

    private void TestMerging(params int[][] values)
    {
        var store = NewStore();

        foreach (var intermediateValues in values)
        {
            var intermediateStore = NewStore();

            foreach (var value in intermediateValues)
            {
                intermediateStore.Add(value);
            }

            store.MergeWith(intermediateStore);
        }

        Test(values.SelectMany(i => i).Select(i => new Bin(i, 1)).ToArray(), store);
        TestMerging(values.Select(v => v.Select(i => new Bin(i, 1)).ToArray()).ToArray());
    }

    private void TestMerging(params Bin[][] bins)
    {
        var store1 = NewStore();
        var store2 = NewStore();

        foreach (var intermediateBins in bins)
        {
            var intermediateStore1 = NewStore();
            var intermediateStore2 = NewStore();

            foreach (var bin in intermediateBins)
            {
                intermediateStore1.Add(bin);
                intermediateStore2.Add(bin.Index, bin.Count);
            }

            store1.MergeWith(intermediateStore1);
            store2.MergeWith(intermediateStore2);
        }

        Test(bins.SelectMany(b => b).ToArray(), store1);
        Test(bins.SelectMany(b => b).ToArray(), store2);
    }

    private void Test(Bin[] bins, Store store)
    {
        var expectedNonZeroCounts = GetNonZeroCounts(GetCounts(bins));
        AssertEncodes(expectedNonZeroCounts, store);

        // TODO: Test protobuf round-trip
    }
}
