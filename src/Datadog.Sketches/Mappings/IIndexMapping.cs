// <copyright file="IIndexMapping.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using Datadog.Sketches.Serialization;

#pragma warning disable SA1600 // Elements should be documented

namespace Datadog.Sketches.Mappings;

public interface IIndexMapping : ISerializable
{
    double RelativeAccuracy { get; }

    double MinIndexableValue { get; }

    double MaxIndexableValue { get; }

    int GetIndex(double value);

    double GetValue(int index);

    double GetLowerBound(int index);
}
