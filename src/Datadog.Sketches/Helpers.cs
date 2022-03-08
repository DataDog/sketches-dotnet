// <copyright file="Helpers.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

namespace Datadog.Sketches;

internal static class Helpers
{
    public static void ArrayFill<T>(T[] array, int fromIndex, int toIndex, T value)
    {
#if NETCOREAPP2_0_OR_GREATER
            System.Array.Fill(array, value, fromIndex, toIndex - fromIndex);
#else
            for (var i = fromIndex; i < toIndex; i++)
            {
                array[i] = value;
            }
#endif
    }
}
