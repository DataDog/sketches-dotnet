// <copyright file="ProtobufHelpers.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Linq;
using Datadog.Sketches.Mappings;
using Datadog.Sketches.Stores;

namespace Datadog.Sketches.Tests
{
    internal class ProtobufHelpers
    {
        internal static DDSketch FromProto(DDSketchProto.DDSketchProto sketch, Func<Store> storeFactory)
        {
            return new DDSketch(
                FromProto(sketch.Mapping),
                FromProto(sketch.NegativeValues, storeFactory),
                FromProto(sketch.PositiveValues, storeFactory),
                minIndexedValue: 0,
                zeroCount: sketch.ZeroCount);
        }

        internal static IIndexMapping FromProto(DDSketchProto.IndexMapping mapping)
            => new LogarithmicMapping(mapping.Gamma, mapping.IndexOffset);

        internal static Store FromProto(DDSketchProto.Store store, Func<Store> storeFactory)
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

        internal static DDSketchProto.DDSketchProto ToProto(DDSketch sketch)
        {
            return new DDSketchProto.DDSketchProto
            {
                PositiveValues = ToProto(sketch.PositiveValueStore),
                NegativeValues = ToProto(sketch.NegativeValueStore),
                ZeroCount = sketch.ZeroCount,
                Mapping = ToProto(sketch.IndexMapping)
            };
        }

        internal static DDSketchProto.IndexMapping ToProto(IIndexMapping mapping)
        {
            var logMapping = (LogarithmicMapping)mapping;

            return new DDSketchProto.IndexMapping
            {
                Gamma = logMapping.Gamma,
                IndexOffset = logMapping.IndexOffset,
                Interpolation = DDSketchProto.IndexMapping.Types.Interpolation.None
            };
        }

        internal static DDSketchProto.Store ToProto(Store store)
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
