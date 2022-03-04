// <copyright file="RelativeAccuracyTester.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using FluentAssertions;

namespace Datadog.Sketches.Tests;

internal class RelativeAccuracyTester
{
    internal const double FloatingPointAcceptableError = 1e-10;

    public static void AssertAccurate(double maxExpected, double actual)
    {
        actual.Should().BeLessThanOrEqualTo(maxExpected + FloatingPointAcceptableError);
    }

    public static double Compute(double expected, double actual) => Compute(expected, expected, actual);

    public static double Compute(double expectedMin, double expectedMax, double actual)
    {
        if (expectedMin < 0 || expectedMax < 0 || actual < 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        if ((expectedMin <= actual) && (actual <= expectedMax))
        {
            return 0;
        }

        if (expectedMin == 0 && expectedMax == 0)
        {
            return actual == 0 ? 0 : double.PositiveInfinity;
        }

        if (actual < expectedMin)
        {
            return (expectedMin - actual) / expectedMin;
        }

        return (actual - expectedMax) / expectedMax;
    }
}
