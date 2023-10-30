# ASP.NET Core `http.route`

There are a number of different APIs available for retrieving information about
the matched route in ASP.NET Core.

## Retrieving the route template

### [RoutePattern.RawText](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.routing.patterns.routepattern.rawtext)

```csharp
(httpContext.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText;
```

### [IRouteDiagnosticsMetadata.Route](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.metadata.iroutediagnosticsmetadata.route)

```csharp
httpContext.GetEndpoint()?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;
```

## HttpContext.GetRouteData()

```csharp
foreach (var value in context.GetRouteData().Values)
{
    Console.WriteLine($"{value.Key} = {value.Value?.ToString()}");
}
```

## Information from the ActionDescriptor

### [AttributeRouteInfo.Template](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.routing.attributerouteinfo.template)

```csharp
actionDescriptor.AttributeRouteInfo?.Template;
```

### [Parameters]

```csharp
actionDescriptor.Parameters;
```

### [ControllerActionDescriptor]

```csharp
(actionDescriptor as ControllerActionDescriptor)?.ControllerName;
(actionDescriptor as ControllerActionDescriptor)?.ActionName;
```

### [PageActionDescriptor]

```csharp
(actionDescriptor as PageActionDescriptor)?.RelativePath;
(actionDescriptor as PageActionDescriptor)?.ViewEnginePath;
```
