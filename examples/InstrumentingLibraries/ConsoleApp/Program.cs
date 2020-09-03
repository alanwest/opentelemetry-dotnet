// <copyright file="Program.cs" company="OpenTelemetry Authors">
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
using System.Diagnostics;
using LegacyInstrumentedLibrary.Instrumentation;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace ConsoleApp
{
    public class Program
    {
        private const string ActivitySourceName = "console-app";

        public static void Main(string[] args)
        {
            Console.WriteLine("Running ActivitySource example");
            Console.WriteLine("------------------------------");
            RunActivitySourceExample();

            Console.WriteLine("Running legacy example");
            Console.WriteLine("----------------------");
            RunLegacyExample();
        }

        private static void RunActivitySourceExample()
        {
            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySourceName)
                .AddSource(ActivitySourceInstrumentedLibrary.ClassA.ActivitySourceName)
                .AddSource(ActivitySourceInstrumentedLibrary.ClassB.ActivitySourceName)
                .AddConsoleExporter()
                .Build();

            var source = new ActivitySource(ActivitySourceName);

            using (var parent = source.StartActivity("Main"))
            {
                var classA = new ActivitySourceInstrumentedLibrary.ClassA();
                classA.ClassAMethod();
            }
        }

        private static void RunLegacyExample()
        {
            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySourceName)
                .AddInstrumentation(activitySourceAdapter => new LegacyInstrumentedLibraryInstrumentation(activitySourceAdapter))
                .AddConsoleExporter()
                .Build();

            var source = new ActivitySource(ActivitySourceName);

            using (var parent = source.StartActivity("Main"))
            {
                var myClass = new LegacyInstrumentedLibrary.ClassA();
                myClass.ClassAMethod();
            }
        }
    }
}
