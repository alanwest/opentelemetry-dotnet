// <copyright file="UnobservedTaskRepro.cs" company="OpenTelemetry Authors">
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

#if !NETFRAMEWORK
using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Exporter.OpenTelemetryProtocol.Tests;

public sealed class UnobservedTaskRepro
{
    [Fact]
    public void UnobservedTaskReproTest()
    {
        Exception unobservedTaskException = null;

        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            unobservedTaskException = e.Exception;
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(nameof(this.UnobservedTaskReproTest))
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://my-collector");
                options.ExportProcessorType = ExportProcessorType.Simple;
            })
            .Build();

        using var source = new ActivitySource(nameof(this.UnobservedTaskReproTest));
        source.StartActivity().Stop();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Assert.Null(unobservedTaskException);
    }
}
#endif
