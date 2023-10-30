# Test results for ASP.NET Core 7

| | | display name | expected name (w/o http.method) | routing type | request |
| - | - | - | - | - | - |
| :broken_heart: | [1](#1) | /ConventionalRoute/NotFound |  | ConventionalRouting | GET /ConventionalRoute/NotFound | /ConventionalRoute/NotFound |
| :broken_heart: | [2](#2) | /SomePath/SomeString/NotAnInt |  | ConventionalRouting | GET /SomePath/SomeString/NotAnInt | /SomePath/SomeString/NotAnInt |
| :green_heart: | [3](#3) | AttributeRoute | AttributeRoute | AttributeRouting | GET /AttributeRoute | AttributeRoute |
| :green_heart: | [4](#4) | AttributeRoute/Get | AttributeRoute/Get | AttributeRouting | GET /AttributeRoute/Get | AttributeRoute/Get |
| :green_heart: | [5](#5) | AttributeRoute/Get/{id} | AttributeRoute/Get/{id} | AttributeRouting | GET /AttributeRoute/Get/12 | AttributeRoute/Get/{id} |
| :green_heart: | [6](#6) | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRouting | GET /AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate |
| :green_heart: | [7](#7) | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate | AttributeRouting | GET /AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate | AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate |
| :broken_heart: | [8](#8) | /js/site.js |  | RazorPages | GET /js/site.js | /js/site.js |

## 1

```json
{
  "HttpMethod": "GET",
  "Path": "/ConventionalRoute/NotFound",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": null,
  "RouteSummary": {
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```

## 2

```json
{
  "HttpMethod": "GET",
  "Path": "/SomePath/SomeString/NotAnInt",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": null,
  "RouteSummary": {
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```

## 3

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute",
  "HttpRouteByRawText": "AttributeRoute",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get",
  "HttpRouteByActionDescriptor": "AttributeRoute",
  "RouteSummary": {
    "RoutePattern.RawText": "AttributeRoute",
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```

## 4

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/Get",
  "HttpRouteByRawText": "AttributeRoute/Get",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get",
  "HttpRouteByActionDescriptor": "AttributeRoute/Get",
  "RouteSummary": {
    "RoutePattern.RawText": "AttributeRoute/Get",
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```

## 5

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/Get/12",
  "HttpRouteByRawText": "AttributeRoute/Get/{id}",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/Get/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/Get/{id}",
  "RouteSummary": {
    "RoutePattern.RawText": "AttributeRoute/Get/{id}",
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get/{id}",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```

## 6

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByRawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteSummary": {
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```

## 7

```json
{
  "HttpMethod": "GET",
  "Path": "/AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByRawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "HttpRouteByControllerActionAndParameters": "AttributeRoute/GetWithActionNameInDifferentSpotInTemplate/{id}",
  "HttpRouteByActionDescriptor": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteSummary": {
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "NotAnInt"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```

## 8

```json
{
  "HttpMethod": "GET",
  "Path": "/js/site.js",
  "HttpRouteByRawText": null,
  "HttpRouteByControllerActionAndParameters": "",
  "HttpRouteByActionDescriptor": null,
  "RouteSummary": {
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
