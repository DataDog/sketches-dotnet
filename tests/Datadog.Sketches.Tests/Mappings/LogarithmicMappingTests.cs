// <copyright file="LogarithmicMappingTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using Datadog.Sketches.Mappings;
using FluentAssertions;
using NUnit.Framework;

namespace Datadog.Sketches.Tests.Mappings;

public class LogarithmicMappingTests
{
    private const double MinTestedRelativeAccuracy = 1e-8;
    private const double MaxTestedRelativeAccuracy = 1 - 1e-3;

    private static readonly double Multiplier = 1 + (Math.Sqrt(2) * 1e-1);

    private static readonly double[] TestGammas = { 1 + 1e-6, 1.02, 1.5 };
    private static readonly double[] TestIndexOffsets = { 0, 1, -12.23, 7768.3 };

    [Test]
    public void TestAccuracy()
    {
        for (var relativeAccuracy = MaxTestedRelativeAccuracy;
            relativeAccuracy >= MinTestedRelativeAccuracy;
            relativeAccuracy *= MaxTestedRelativeAccuracy)
        {
            TestAccuracy(new LogarithmicMapping(relativeAccuracy), relativeAccuracy);
        }

        foreach (var gamma in TestGammas)
        {
            foreach (var indexOffset in TestIndexOffsets)
            {
                var mapping = new LogarithmicMapping(gamma, indexOffset);
                TestAccuracy(mapping, mapping.RelativeAccuracy);
            }
        }
    }

    [Test]
    public void TestOffset()
    {
        foreach (var gamma in TestGammas)
        {
            foreach (var indexOffset in TestIndexOffsets)
            {
                var mapping = new LogarithmicMapping(gamma, indexOffset);
                TestOffset(mapping, indexOffset);
            }
        }
    }

    [Test]
    public void TestProtoRoundTrip()
    {
        // TODO: Add test when serialization is implemented
        Assert.Inconclusive();
    }

    [Test]
    public void TestEncodeDecode()
    {
        // TODO: Add test when serialization is implemented
        Assert.Inconclusive();
    }

    private static void AssertRelativelyAccurate(IIndexMapping mapping, double value)
    {
        var relativeAccuracy = RelativeAccuracyTester.Compute(value, mapping.GetValue(mapping.GetIndex(value)));

        RelativeAccuracyTester.AssertAccurate(mapping.RelativeAccuracy, relativeAccuracy);
    }

    private static void TestAccuracy(IIndexMapping mapping, double relativeAccuracy)
    {
        // Assert that the stated relative accuracy of the mapping is less than or equal to the requested one.
        RelativeAccuracyTester.AssertAccurate(relativeAccuracy, mapping.RelativeAccuracy);

        AssertRelativelyAccurate(mapping);
    }

    private static void AssertRelativelyAccurate(IIndexMapping mapping)
    {
        for (var value = mapping.MinIndexableValue; value < mapping.MaxIndexableValue; value *= Multiplier)
        {
            AssertRelativelyAccurate(mapping, value);
        }

        AssertRelativelyAccurate(mapping, mapping.MaxIndexableValue);
    }

    private static void TestOffset(IIndexMapping mapping, double indexOffset)
    {
        double indexOf1 = mapping.GetIndex(1);

        // If 1 is on a bucket boundary, its associated index can be either of the ones of the previous and the next buckets.
        indexOf1.Should().BeGreaterThanOrEqualTo(Math.Ceiling(indexOffset) - 1);
        indexOf1.Should().BeLessThanOrEqualTo(Math.Floor(indexOffset));
    }
}
