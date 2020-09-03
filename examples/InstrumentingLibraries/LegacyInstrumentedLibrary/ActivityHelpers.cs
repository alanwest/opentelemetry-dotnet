// <copyright file="ActivityHelpers.cs" company="OpenTelemetry Authors">
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

using System.Diagnostics;

namespace LegacyInstrumentedLibrary
{
    internal static class ActivityHelpers
    {
        public static readonly string SourceName = $"{nameof(LegacyInstrumentedLibrary)}";
        private static readonly DiagnosticListener DiagnosticListener = new DiagnosticListener(SourceName);

        public static Activity StartActivity(string activityName, object payload)
        {
            Activity activity = null;

            var diagnosticSourceEnabled = IsDiagnosticSourceEnabled(activityName, payload);

            if (diagnosticSourceEnabled || Activity.Current != null)
            {
                activity = new Activity(activityName);

                if (diagnosticSourceEnabled)
                {
                    DiagnosticListener.StartActivity(activity, payload);
                }
                else
                {
                    activity.Start();
                }
            }

            return activity;
        }

        public static void StopActivity(string activityName, Activity activity, object payload)
        {
            if (activity != null)
            {
                if (IsDiagnosticSourceEnabled(activityName, payload))
                {
                    DiagnosticListener.StopActivity(activity, payload);
                }
                else
                {
                    activity.Stop();
                }
            }
        }

        private static bool IsDiagnosticSourceEnabled(string activityName, object payload)
        {
            return DiagnosticListener.IsEnabled() && DiagnosticListener.IsEnabled(activityName, payload);
        }
    }
}
