﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Envivo.Fresnel.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // This allows non-standard REST methods to be invoked:
            var routeTemplate = "api/{controller}/{action}/{id}";

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: routeTemplate,
                defaults: new { id = RouteParameter.Optional }
            );

            // Make sure we return JSON from any API calls:
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
