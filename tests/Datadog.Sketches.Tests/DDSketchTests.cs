// <copyright file="DDSketchTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Datadog.Sketches.Mappings;
using Datadog.Sketches.Stores;
using FluentAssertions;
using NUnit.Framework;

namespace Datadog.Sketches.Tests
{
    public class DDSketchTests
    {
        internal const double FloatingPointAcceptableError = 1e-10;

        public static IEnumerable<double[]> GetConstantTestCases()
        {
            // Positive
            yield return new double[] { 0 };
            yield return new double[] { 1 };
            yield return new double[] { 10 };
            yield return new double[] { 1, 1, 1 };
            yield return new double[] { 10, 10, 10 };
            yield return Enumerable.Repeat(2.0, 10_000).ToArray();
            yield return new double[] { 10, 10, 11, 11, 11 };

            // Negative
            yield return new double[] { -1 };
            yield return new double[] { -1, -1, -1 };
            yield return new double[] { -10, -10, -10 };
            yield return Enumerable.Repeat(-2.0, 10_000).ToArray();
            yield return new double[] { -10, -10, -11, -11, -11 };

            // Mixed negative and positive
            yield return new double[] { -1, 1 };
            yield return new double[] { -1, -1, -1, 1, 1, 1 };
            yield return new double[] { -10, -10, -10, 10, 10, 10 };
            yield return Enumerable.Range(0, 20_000).Select(i => i % 2 == 0 ? 2.0 : -2.0).ToArray();
            yield return new double[] { -10, -10, -11, -11, -11, 10, 10, 11, 11, 11 };

            // With zeroes
            yield return Enumerable.Repeat(0.0, 100).ToArray();
            yield return Enumerable.Repeat(0.0, 10).Concat(DoubleRange(0, 100)).ToArray();
            yield return DoubleRange(0, 100).Concat(Enumerable.Repeat(0.0, 10)).ToArray();
            yield return Enumerable.Repeat(0.0, 10).Concat(DoubleRange(-100, 200)).ToArray();
            yield return DoubleRange(-100, 200).Concat(Enumerable.Repeat(0.0, 10)).ToArray();

            // Without zeroes
            yield return DoubleRange(1, 99).ToArray();
            yield return DoubleRange(-100, 99).Concat(DoubleRange(1, 99)).ToArray();

            // Increasing linearly
            yield return DoubleRange(0, 10_000).ToArray();

            // Decreasing linearly
            yield return DoubleRange(0, 10_000).Select(v => 10_000 - v).ToArray();

            // Negative numbers increasing linearly
            yield return DoubleRange(-10_000, 10_000).ToArray();

            // Negative and positive numbers increasing linearly
            yield return DoubleRange(-10_000, 20_000).ToArray();

            // Negative numbers decreasing linearly
            yield return DoubleRange(0, 10_000).Select(v => -v).ToArray();

            // Negative and positive numbers decreasing linearly
            yield return DoubleRange(0, 20_000).Select(v => 10_000 - v).ToArray();

            // Increasing exponentially
            yield return DoubleRange(0, 100).Select(Math.Exp).ToArray();

            // Decreasing exponentially
            yield return DoubleRange(0, 100).Select(v => Math.Exp(-v)).ToArray();

            // Negative numbers increasing exponentially
            yield return DoubleRange(0, 100).Select(v => -Math.Exp(v)).ToArray();

            // Negative and positive numbers increasing exponentially
            yield return DoubleRange(0, 100).Select(v => -Math.Exp(v))
                .Concat(DoubleRange(0, 100).Select(Math.Exp))
                .ToArray();

            // Negative numbers decreasing exponentially
            yield return DoubleRange(0, 100).Select(v => -Math.Exp(-v)).ToArray();

            // Negative and positive numbers decreading exponentially
            yield return DoubleRange(0, 100).Select(v => -Math.Exp(-v))
                .Concat(DoubleRange(0, 100).Select(v => Math.Exp(-v)))
                .ToArray();
        }

        [Test]
        public void ThrowsExceptionsWhenExpected()
        {
            var sketch = NewSketch();
            EmptySketchAssertions(sketch);

            sketch.Add(0);
            NonEmptySketchAssertions(sketch);
        }

        [Test]
        public void ClearSketchShouldBehaveEmpty()
        {
            var sketch = NewSketch();
            sketch.Add(0);
            sketch.Clear();
            sketch.IsEmpty().Should().BeTrue();
            EmptySketchAssertions(sketch);
        }

        [Test]
        public void TestEmpty()
        {
            TestAdding();
        }

        [Test]
        [TestCaseSource(nameof(GetConstantTestCases))]
        public void TestConstants(double[] values)
        {
            TestAdding(values);
        }

        [Test]
        public void TestMergingConstants()
        {
            TestMerging(new[] { 1.0, 1.0 }, new[] { 1.0, 1.0, 1.0 });
        }

        [Test]
        [TestCase(new[] { 0.0 }, new[] { 10_000.0 }, null)]
        [TestCase(new[] { 10_000.0 }, new[] { 20_000.0 }, null)]
        [TestCase(new[] { 20_000.0 }, new[] { 10_000.0 }, null)]
        [TestCase(new[] { 10_000.0 }, new[] { 0.0 }, new[] { 0.0 })]
        [TestCase(new[] { 10_000.0, 0.0 }, new[] { 10_000.0 }, new[] { 0.0 })]
        public void TestMergingFarApart(double[] values1, double[] values2, double[] values3)
        {
            TestMerging(values1, values2, values3 ?? Array.Empty<double>());
        }

        private static IEnumerable<double> DoubleRange(double start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return start++;
            }
        }

        private void EmptySketchAssertions(DDSketch sketch)
        {
            sketch.GetSum().Should().Be(0);
            sketch.GetCount().Should().Be(0);
            sketch.Invoking(s => s.GetMinValue()).Should().Throw<InvalidOperationException>();
            sketch.Invoking(s => s.GetMaxValue()).Should().Throw<InvalidOperationException>();
            sketch.Invoking(s => s.GetValueAtQuantile(0.5)).Should().Throw<InvalidOperationException>();
            sketch.Invoking(s => s.GetValuesAtQuantiles(new[] { 0.5 })).Should().Throw<InvalidOperationException>();
            sketch.Invoking(s => s.MergeWith(null)).Should().Throw<ArgumentNullException>();
            sketch.Invoking(s => s.Add(0, -1)).Should().Throw<ArgumentOutOfRangeException>();
            sketch.Invoking(s => s.Add(1, -1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        private void NonEmptySketchAssertions(DDSketch sketch)
        {
            sketch.Invoking(s => s.GetValueAtQuantile(-0.1)).Should().Throw<ArgumentOutOfRangeException>();
            sketch.Invoking(s => s.GetValueAtQuantile(1.1)).Should().Throw<ArgumentOutOfRangeException>();
            sketch.Invoking(s => s.GetValuesAtQuantiles(new[] { 0, -0.1 }).ToArray()).Should().Throw<ArgumentOutOfRangeException>();
            sketch.Invoking(s => s.GetValuesAtQuantiles(new[] { 1.1, 1 }).ToArray()).Should().Throw<ArgumentOutOfRangeException>();
        }

        private void TestMerging(params double[][] values)
        {
            var sketch = NewSketch();

            foreach (var sketchValues in values)
            {
                var intermediateSketch = NewSketch();

                foreach (var value in sketchValues)
                {
                    intermediateSketch.Add(value);
                }

                sketch.MergeWith(intermediateSketch);
            }

            Test(true, values.SelectMany(v => v).ToArray(), sketch);
        }

        private void TestAdding(params double[] values)
        {
            var sketch = NewSketch();

            foreach (var value in values)
            {
                sketch.Add(value);
            }

            Test(false, values, sketch);
        }

        private DDSketch NewSketch()
        {
            return new DDSketch(new LogarithmicMapping(RelativeAccuracy()), new UnboundedSizeDenseStore(), new UnboundedSizeDenseStore());
        }

        private void Test(bool merged, double[] values, DDSketch sketch)
        {
            AssertEncodes(merged, values, sketch);
        }

        private void AssertEncodes(bool merged, double[] values, DDSketch sketch)
        {
            sketch.GetCount().Should().BeApproximately(values.Length, double.Epsilon);

            if (values.Length == 0)
            {
                sketch.IsEmpty().Should().BeTrue();
            }
            else
            {
                sketch.IsEmpty().Should().BeFalse();

                var sortedValues = values.OrderBy(v => v).ToArray();

                var minValue = sketch.GetMinValue();
                var maxValue = sketch.GetMaxValue();

                AssertMinAccurate(sortedValues, minValue);
                AssertMaxAccurate(sortedValues, maxValue);

                for (double quantile = 0; quantile <= 1; quantile += 0.01)
                {
                    var valueAtQuantile = sketch.GetValueAtQuantile(quantile);
                    AssertQuantileAccurate(merged, sortedValues, quantile, valueAtQuantile);

                    valueAtQuantile.Should().BeGreaterThanOrEqualTo(minValue);
                    valueAtQuantile.Should().BeLessThanOrEqualTo(maxValue);

                    var valuesAtQuantiles = sketch.GetValuesAtQuantiles(new[] { quantile }).ToArray();
                    valuesAtQuantiles.Should().HaveCount(1);
                    valuesAtQuantiles[0].Should().Be(valueAtQuantile);
                }

                AssertSumAccurate(values, sketch.GetSum());
            }
        }

        private void AssertQuantileAccurate(bool merged, double[] sortedValues, double quantile, double actualQuantileValue)
            => AssertQuantileAccurate(sortedValues, quantile, actualQuantileValue, RelativeAccuracy());

        private void AssertQuantileAccurate(double[] sortedValues, double quantile, double actualQuantileValue, double relativeAccuracy)
        {
            var lowerQuantileValue = sortedValues[(int)Math.Floor(quantile * (sortedValues.Length - 1))];
            var upperQuantileValue = sortedValues[(int)Math.Ceiling(quantile * (sortedValues.Length - 1))];

            AssertAccurate(lowerQuantileValue, upperQuantileValue, actualQuantileValue, relativeAccuracy);
        }

        private void AssertMinAccurate(double[] sortedValues, double actualMinValue) => AssertAccurate(sortedValues.First(), actualMinValue);

        private void AssertMaxAccurate(double[] sortedValues, double actualMaxValue) => AssertAccurate(sortedValues.Last(), actualMaxValue);

        private void AssertSumAccurate(double[] sortedValues, double actualSumValue)
        {
            // The sum is accurate if the values that have been added to the sketch have same sign.
            if (SameSign(sortedValues))
            {
                AssertAccurate(sortedValues.Sum(), actualSumValue);
            }
        }

        private void AssertAccurate(double expected, double actual) => AssertAccurate(expected, expected, actual);

        private void AssertAccurate(double minExpected, double maxExpected, double actual) => AssertAccurate(minExpected, maxExpected, actual, RelativeAccuracy());

        private void AssertAccurate(double minExpected, double maxExpected, double actual, double relativeAccuracy)
        {
            var relaxedMinExpected =
                minExpected > 0
                    ? minExpected * (1 - relativeAccuracy)
                    : minExpected * (1 + relativeAccuracy);

            var relaxedMaxExpected =
                maxExpected > 0
                    ? maxExpected * (1 + relativeAccuracy)
                    : maxExpected * (1 - relativeAccuracy);

            actual.Should()
                .BeGreaterThanOrEqualTo(relaxedMinExpected - FloatingPointAcceptableError)
                .And.BeLessThanOrEqualTo(relaxedMaxExpected + FloatingPointAcceptableError);
        }

        private double RelativeAccuracy() => 1e-1;

        private bool SameSign(double[] sortedValues)
        {
            // To find out if the sign is the same, we only need to test the first and last value.
            // For instance, if the first value is positive, since they are sorted the next ones are guaranteed to be also positive.
            return sortedValues.First() >= 0 || sortedValues.Last() <= 0;
        }
    }
}
