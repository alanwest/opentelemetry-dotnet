// <copyright file="TraceService.cs" company="OpenTelemetry Authors">
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

using Grpc.Core;
using Opentelemetry.Proto.Collector.Trace.V1;

namespace MockOpenTelemetryCollector.Services
{
    internal class TraceService : Opentelemetry.Proto.Collector.Trace.V1.TraceService.TraceServiceBase
    {
        private readonly MockCollectorState state;

        public TraceService(MockCollectorState state)
        {
            Console.WriteLine($"TraceService: state hash {state.GetHashCode()}");
            this.state = state;
        }

        public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
        {
            Console.WriteLine("{0:MM/dd/yyyy H:mm:ss.fff}", DateTime.UtcNow);
            throw new RpcException(new Status(this.state.NextStatus(), "Un oh."));
        }
    }
}
