// <copyright file="SerializerTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datadog.Sketches.Mappings;
using Datadog.Sketches.Stores;
using FluentAssertions;
using Google.Protobuf;
using NUnit.Framework;

namespace Datadog.Sketches.Tests
{
    public class SerializerTests
    {
        public static IEnumerable<object[]> TestCases()
        {
            return from store in Stores()
                   from distribution in Distributions()
                   select new object[] { store, distribution };
        }

        public static IEnumerable<Func<Store>> Stores()
        {
            yield return () => new CollapsingLowestDenseStore(1000);
            yield return () => new CollapsingHighestDenseStore(1000);
            yield return () => new UnboundedSizeDenseStore();
        }

        public static IEnumerable<IEnumerable<double>> Distributions()
        {
            yield return Tests.Distributions.Normal(0, 1);
            yield return Tests.Distributions.Normal(100, 10);
            yield return Tests.Distributions.Poisson(0.01);
            yield return Tests.Distributions.Poisson(0.99);
            yield return Tests.Distributions.Point(42);
            yield return Tests.Distributions.Uniform(100);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestProtobufSerialization(Func<Store> storeFactory, IEnumerable<double> distribution)
        {
            var sketch = new DDSketch(new LogarithmicMapping(0.01), storeFactory(), storeFactory());

            foreach (var value in distribution.Take(10_000))
            {
                sketch.Add(value);
            }

            using (var stream = new MemoryStream())
            {
                sketch.Serialize(stream);

                AssertEquals(sketch, stream.ToArray());
            }

            sketch.Clear();

            using (var stream = new MemoryStream())
            {
                sketch.Serialize(stream);

                AssertEquals(sketch, stream.ToArray());
            }
        }

        private void AssertEquals(DDSketch sketch, byte[] buffer)
        {
            var protoSketch = ToProto(sketch);

            using (var stream = new MemoryStream())
            {
                using (var codedOutputStream = new CodedOutputStream(stream))
                {
                    protoSketch.WriteTo(codedOutputStream);
                }

                buffer.Should().Equal(stream.ToArray());
            }

            var deserialized = DDSketchProto.DDSketchProto.Parser.ParseFrom(buffer);
            var recovered = FromProto(deserialized, () => new UnboundedSizeDenseStore());

            recovered.GetCount().Should().BeApproximately(sketch.GetCount(), RelativeAccuracyTester.FloatingPointAcceptableError);
            recovered.IndexMapping.RelativeAccuracy.Should().BeApproximately(
                sketch.IndexMapping.RelativeAccuracy, RelativeAccuracyTester.FloatingPointAcceptableError);

            recovered.PositiveValueStore.Should().Equal(sketch.PositiveValueStore);
            recovered.NegativeValueStore.Should().Equal(sketch.NegativeValueStore);
        }

        private DDSketch FromProto(DDSketchProto.DDSketchProto sketch, Func<Store> storeFactory)
        {
            return new DDSketch(
                FromProto(sketch.Mapping),
                FromProto(sketch.NegativeValues, storeFactory),
                FromProto(sketch.PositiveValues, storeFactory),
                minIndexedValue: 0,
                zeroCount: sketch.ZeroCount);
        }

        private IIndexMapping FromProto(DDSketchProto.IndexMapping mapping)
            => new LogarithmicMapping(mapping.Gamma, mapping.IndexOffset);

        private Store FromProto(DDSketchProto.Store store, Func<Store> storeFactory)
        {
            var result = storeFactory();

            foreach (var element in store.BinCounts)
            {
                result.Add(element.Key, element.Value);
            }

            var index = store.ContiguousBinIndexOffset;

            foreach (var value in store.ContiguousBinCounts)
            {
                result.Add(index++, value);
            }

            return result;
        }

        private DDSketchProto.DDSketchProto ToProto(DDSketch sketch)
        {
            return new DDSketchProto.DDSketchProto
            {
                PositiveValues = ToProto(sketch.PositiveValueStore),
                NegativeValues = ToProto(sketch.NegativeValueStore),
                ZeroCount = sketch.ZeroCount,
                Mapping = ToProto(sketch.IndexMapping)
            };
        }

        private DDSketchProto.IndexMapping ToProto(IIndexMapping mapping)
        {
            var logMapping = (LogarithmicMapping)mapping;

            return new DDSketchProto.IndexMapping
            {
                Gamma = logMapping.Gamma,
                IndexOffset = logMapping.IndexOffset,
                Interpolation = DDSketchProto.IndexMapping.Types.Interpolation.None
            };
        }

        private DDSketchProto.Store ToProto(Store store)
        {
            var denseStore = (DenseStore)store;

            var protoStore = new DDSketchProto.Store();

            if (!store.IsEmpty())
            {
                protoStore.ContiguousBinIndexOffset = denseStore.MinIndex;

                protoStore.ContiguousBinCounts.AddRange(denseStore.Counts
                    .Skip(denseStore.MinIndex - denseStore.Offset)
                    .Take(denseStore.MaxIndex - denseStore.MinIndex + 1));
            }

            return protoStore;
        }
    }
}
