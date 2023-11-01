// <copyright file="RoutingTestFixture.cs" company="OpenTelemetry Authors">
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

#nullable enable

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;

namespace RouteTests;

public class RoutingTestFixture : IDisposable
{
    private readonly Dictionary<TestApplicationScenario, WebApplication> apps = new();
    private readonly HttpClient client = new();
    private readonly RouteInfoDiagnosticObserver diagnostics = new();
    private readonly List<TestResult> testResults = new();

    public RoutingTestFixture()
    {
        foreach (var scenario in Enum.GetValues<TestApplicationScenario>())
        {
            var app = TestApplicationFactory.CreateApplication(scenario);
            if (app != null)
            {
                this.apps.Add(scenario, app);
            }
        }

        foreach (var app in this.apps)
        {
            app.Value.RunAsync();
        }
    }

    public async Task MakeRequest(TestApplicationScenario scenario, string path)
    {
        var app = this.apps[scenario];
        var baseUrl = app.Urls.First();
        var url = $"{baseUrl}{path}";
        await this.client.GetAsync(url).ConfigureAwait(false);
    }

    public void AddTestResult(TestResult testResult)
    {
        var app = this.apps[testResult.TestCase.TestApplicationScenario];
        var baseUrl = app.Urls.First();
        var url = $"{baseUrl}/GetLastRouteInfo";
        var responseMessage = this.client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
        var response = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var info = JsonSerializer.Deserialize<RouteInfo>(response);
        testResult.RouteInfo = info!;
        this.testResults.Add(testResult);
    }

    public void Dispose()
    {
        foreach (var app in this.apps)
        {
            app.Value.DisposeAsync().GetAwaiter().GetResult();
        }

        this.client.Dispose();
        this.diagnostics.Dispose();

        this.GenerateReadme();
    }

    private void GenerateReadme()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# Test results for ASP.NET Core {Environment.Version.Major}");
        sb.AppendLine();
        sb.AppendLine("| ADN | AR | MR | App | Test Name |");
        sb.AppendLine("| - | - | - | - | - |");

        for (var i = 0; i < this.testResults.Count; ++i)
        {
            var result = this.testResults[i];
            var emoji1 = result.TestCase.CurrentActivityDisplayName == null ? ":green_heart:" : ":broken_heart:";
            var emoji2 = result.TestCase.CurrentActivityHttpRoute == null ? ":green_heart:" : ":broken_heart:";
            var emoji3 = result.TestCase.CurrentMetricHttpRoute == null ? ":green_heart:" : ":broken_heart:";
            sb.Append($"| {emoji1} | {emoji2} | {emoji3} | ");
            sb.AppendLine($" {result.TestCase.TestApplicationScenario} | {result.TestCase.Name} |");
        }

        for (var i = 0; i < this.testResults.Count; ++i)
        {
            var result = this.testResults[i];
            sb.AppendLine();
            sb.AppendLine($"## {result.TestCase.TestApplicationScenario}: {result.TestCase.Name}");
            sb.AppendLine();
            sb.AppendLine("```json");
            sb.AppendLine(result.ToString());
            sb.AppendLine("```");
        }

        var readmeFileName = $"README.net{Environment.Version.Major}.0.md";
        File.WriteAllText(Path.Combine("..", "..", "..", "RouteTests", readmeFileName), sb.ToString());
    }
}
