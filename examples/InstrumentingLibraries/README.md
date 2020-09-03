# Instrumenting libraries with OpenTelemetry

Included here are two examples of instrumenting libraries with OpenTelemetry.

1. `ActivitySourceInstrumentedLibrary` is instrumented using the new
   `ActivitySource` API.
    * This library takes a dependency on a preview package of
    [`System.Diagnostics.DiagnosticSource`](https://www.nuget.org/packages/System.Diagnostics.DiagnosticSource)
    which exposes the new `ActivitySource` API.
    * Generally, no dependency on OpenTelemetry is required to instrumentation
    a library with `ActivitySource`, but one is included in order to
    demonstrate the ability to suppress instrumentation. Suppressing
    instrumentation is a feature of the OpenTelemetry SDK.

1. `LegacyInstrumentedLibrary` is instrumented using `DiagnosticListener`.
    * Separate OpenTelemetry instrumentation needs to be provided for this
    library and requires a dependency on the OpenTelemetry API. Often the
    OpenTelemetry instrumentation would not be in the library itself, but
    in this example it is in the `LegacyInstrumentedLibrary.Instrumentation`
    namespace.

A console application is provided demonstrating the instrumentation.

Run it:

```shell
cd ConsoleApp
dotnet run
```
