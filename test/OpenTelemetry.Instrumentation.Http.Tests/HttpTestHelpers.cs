// <copyright file="TestHttpServerHelper.cs" company="OpenTelemetry Authors">
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
using System.Text;
using System.Text.RegularExpressions;
using OpenTelemetry.Tests;

namespace OpenTelemetry.Instrumentation.Http.Tests;

public static class HttpTestHelpers
{
    private static readonly Regex TraceparentRegex = new("^00-(?<traceid>[a-f0-9]{32})-(?<spanid>[a-f0-9]{16})-(?<traceflags>0[0-1])$", RegexOptions.Compiled);

    public static bool TryCreateActivityContextFromTraceparent(string traceparent, out ActivityContext activityContext)
    {
        var match = TraceparentRegex.Match(traceparent);

        activityContext = match.Success
            ? new ActivityContext(
                ActivityTraceId.CreateFromString(match.Groups["traceid"].Value),
                ActivitySpanId.CreateFromString(match.Groups["spanid"].Value),
                match.Groups["traceflags"].Value == "01" ? ActivityTraceFlags.Recorded : ActivityTraceFlags.None,
                traceState: null,
                isRemote: false)
            : default;

        return activityContext != default;
    }

    public static void CreateTestHttpServer(out string url, out IDisposable serverHandle)
    {
        serverHandle = TestHttpServer.RunServer(
            (ctx) =>
            {
                string traceparent = ctx.Request.Headers["traceparent"];
                string custom_traceparent = ctx.Request.Headers["custom_traceparent"];
                if (string.IsNullOrWhiteSpace(traceparent)
                    && string.IsNullOrWhiteSpace(custom_traceparent))
                {
                    ctx.Response.StatusCode = 500;
                    ctx.Response.StatusDescription = "Missing trace context";
                }
                else if (ctx.Request.Url.PathAndQuery.Contains("500"))
                {
                    ctx.Response.StatusCode = 500;
                }
                else if (ctx.Request.Url.PathAndQuery.Contains("redirect"))
                {
                    ctx.Response.RedirectLocation = "/";
                    ctx.Response.StatusCode = 302;
                }
                else
                {
                    ctx.Response.StatusCode = 200;
                    var tp = traceparent ?? custom_traceparent;
                    ctx.Response.OutputStream.Write(new ReadOnlySpan<byte>(UTF8Encoding.UTF8.GetBytes(tp)));
                }

                ctx.Response.OutputStream.Close();
            },
            out var host,
            out var port);

        url = $"http://{host}:{port}/";
    }
}
