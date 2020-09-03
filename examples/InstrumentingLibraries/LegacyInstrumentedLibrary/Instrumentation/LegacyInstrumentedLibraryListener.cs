// <copyright file="LegacyInstrumentedLibraryListener.cs" company="OpenTelemetry Authors">
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
using OpenTelemetry;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace LegacyInstrumentedLibrary.Instrumentation
{
    internal class LegacyInstrumentedLibraryListener : ListenerHandler
    {
        private readonly ActivitySourceAdapter activitySourceAdapter;

        public LegacyInstrumentedLibraryListener(ActivitySourceAdapter activitySourceAdapter)
            : base(ActivityHelpers.SourceName)
        {
            this.activitySourceAdapter = activitySourceAdapter;
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            Console.WriteLine($"Invoking {nameof(LegacyInstrumentedLibraryListener)}.{nameof(this.OnStartActivity)}");

            this.activitySourceAdapter.Start(activity, ActivityKind.Internal);

            if (activity.IsAllDataRequested)
            {
                activity.SetTag("CustomTag", "CustomValue");
            }
        }

        public override void OnStopActivity(Activity activity, object payload)
        {
            Console.WriteLine($"Invoking {nameof(LegacyInstrumentedLibraryListener)}.{nameof(this.OnStopActivity)}");

            this.activitySourceAdapter.Stop(activity);
        }

        public override void OnException(Activity activity, object payload)
        {
            Console.WriteLine($"Invoking {nameof(LegacyInstrumentedLibraryListener)}.{nameof(this.OnException)}");
        }

        public override void OnCustom(string name, Activity activity, object payload)
        {
            Console.WriteLine($"Invoking {nameof(LegacyInstrumentedLibraryListener)}.{nameof(this.OnCustom)}");
        }
    }
}
