using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SearchResultItemVM : ObjectVM
    {
        public bool IsSelected { get; set; }
    }
}