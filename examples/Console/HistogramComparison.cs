// <copyright file="HistogramComparison.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics.Metrics;
using MathNet.Numerics.Distributions;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

internal class HistogramComparison
{
    internal static object Run()
    {
        using var meter = new Meter("TestMeter");

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HistogramComparison"))
            .AddMeter(meter.Name)
            .AddView((instrument) =>
            {
                if (instrument.GetType().GetGenericTypeDefinition() == typeof(Histogram<>))
                {
                    return instrument.Name.Contains("exponential")
                        ? new ExponentialBucketHistogramConfiguration { MaxSize = 160 }
                        : new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 0, 5, 10, 25, 50, 75, 100, 250, 500, 1000 } };
                }

                return null;
            })
            .AddOtlpExporter((exporterOptions, metricReaderOptions) =>
            {
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 10000;
                metricReaderOptions.TemporalityPreference = MetricReaderTemporalityPreference.Delta;
            })
            .Build();

        var exponentialMs = meter.CreateHistogram<long>("exponential-ms");
        var explicitMs = meter.CreateHistogram<long>("explicit-ms");
        var exponentialNs = meter.CreateHistogram<long>("exponential-ns");
        var explicitNs = meter.CreateHistogram<long>("explicit-ns");

        for (int i = 0; i < 1000000; i++)
        {
            var ms = MeasurementGenerator.GetNextMeasurementMs();
            var ns = ms * 1000000;

            exponentialMs.Record(ms);

            explicitMs.Record(ms);

            // exponentialNs.Record(ns);

            explicitNs.Record(ns);

            if (i == 999998)
            {
                Console.WriteLine("Almost done.");
            }
        }

        return null;
    }

    private class MeasurementGenerator
    {
        private static Random random = new Random();
        private static Gamma gammaDistribution = new Gamma(1.0, 2.0);
        private static Normal middleNormalDistribution = new Normal(250.0, 75.0);
        private static Normal highNormalDistribution = new Normal(800.0, 30.0);
        private static Func<long> lowLatencyGenerator = () => (long)Math.Floor(gammaDistribution.Sample() * 10.0);
        private static Func<long> middleLatencyGenerator = () => (long)Math.Floor(middleNormalDistribution.Sample());
        private static Func<long> highLatencyGenerator = () => (long)Math.Floor(highNormalDistribution.Sample());

        private static Func<long>[] generators = new[]
        {
            lowLatencyGenerator,
            lowLatencyGenerator,
            middleLatencyGenerator,
            middleLatencyGenerator,
            middleLatencyGenerator,
            middleLatencyGenerator,
            middleLatencyGenerator,
            middleLatencyGenerator,
            highLatencyGenerator,
            highLatencyGenerator,
        };

        public static long GetNextMeasurementMs()
        {
            return Math.Max(generators[random.Next(generators.Length)](), 1);
        }
    }
}
