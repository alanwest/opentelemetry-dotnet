// <copyright file="ClassA.cs" company="OpenTelemetry Authors">
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
using System.Threading;
using OpenTelemetry;

namespace ActivitySourceInstrumentedLibrary
{
    public class ClassA
    {
        public static readonly string ActivitySourceName = $"{nameof(ActivitySourceInstrumentedLibrary)}.{nameof(ClassA)}";
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName);

        public void ClassAMethod()
        {
            using var activity = ActivitySource.StartActivity(nameof(this.ClassAMethod));

            if (activity != null && activity.IsAllDataRequested)
            {
                activity?.SetTag("CustomTag", "CustomValue");
            }

            // This will suppress the instrumentation of ClassB.
            // Comment this line out to enable instrumentation of ClassB.
            SuppressInstrumentationScope.Enter();

            var classB = new ClassB();
            classB.ClassBMethod();
        }
    }
}
