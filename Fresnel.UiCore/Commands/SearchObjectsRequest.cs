using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchObjectsRequest : SearchRequest
    {
        public string SearchType { get; set; }

    }
}