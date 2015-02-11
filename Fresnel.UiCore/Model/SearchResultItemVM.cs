using System;
using System.Collections.Generic;
using T4TS;
using Newtonsoft.Json;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SearchResultItemVM : ObjectVM
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsSelected { get; set; }
    }
}