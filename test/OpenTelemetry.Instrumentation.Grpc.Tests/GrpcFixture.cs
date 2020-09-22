// <copyright file="GrpcFixture.cs" company="OpenTelemetry Authors">
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
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Grpc.Tests
{
    public class GrpcFixture<TService> : IDisposable
        where TService : class
    {
        private static readonly Random GlobalRandom = new Random();

        private readonly IHost host;

        private TracerProvider openTelemetrySdk = null;

        public GrpcFixture()
        {
            // Allows gRPC client to call insecure gRPC services
            // https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.1#call-insecure-grpc-services-with-net-core-client
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            this.Port = 0;

            var retryCount = 5;
            while (retryCount > 0)
            {
                try
                {
                    this.Port = GlobalRandom.Next(2000, 5000);
                    this.host = this.CreateServer();
                    this.host.StartAsync().GetAwaiter().GetResult();
                    break;
                }
                catch (System.IO.IOException)
                {
                    retryCount--;
                    this.host.Dispose();
                }
            }
        }

        public int Port { get; }

        public Mock<ActivityProcessor> GrpcServerSpanProcessor { get; } = new Mock<ActivityProcessor>();

        public void Dispose()
        {
            this.host.StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
            this.host.Dispose();
            this.openTelemetrySdk.Dispose();
        }

        private IHost CreateServer()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options =>
                        {
                            // Setup a HTTP/2 endpoint without TLS.
                            options.ListenLocalhost(this.Port, o => o.Protocols = HttpProtocols.Http2);
                        })
                        .ConfigureServices(serviceCollection =>
                        {
                            this.openTelemetrySdk = Sdk
                                .CreateTracerProviderBuilder()
                                        .AddAspNetCoreInstrumentation()
                                        .AddProcessor(this.GrpcServerSpanProcessor.Object)
                                        .Build();
                        })
                        .UseStartup<Startup>();
                });

            return hostBuilder.Build();
        }

        private class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddGrpc();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<TService>();
                });
            }
        }
    }
}
