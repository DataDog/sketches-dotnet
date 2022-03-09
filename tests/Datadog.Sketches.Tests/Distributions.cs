// <copyright file="Distributions.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;

namespace Datadog.Sketches.Tests
{
    internal static class Distributions
    {
        private const int Seed = 538892812;

        public static IEnumerable<double> Point(double parameter)
        {
            while (true)
            {
                yield return parameter;
            }
        }

        public static IEnumerable<double> Uniform(double parameter)
        {
            var random = new Random(Seed);

            while (true)
            {
                yield return parameter * random.NextDouble();
            }
        }

        public static IEnumerable<double> Normal(double mu, double sigma)
        {
            var random = new Random(Seed);

            while (true)
            {
                yield return (sigma * NextGaussian(random)) + mu;
            }
        }

        public static IEnumerable<double> Poisson(double parameter)
        {
            var random = new Random(Seed);

            while (true)
            {
                yield return -(Math.Log(random.NextDouble()) / parameter);
            }
        }

        private static double NextGaussian(Random random)
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();

            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}
