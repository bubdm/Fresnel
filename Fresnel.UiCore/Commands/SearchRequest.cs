using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchRequest
    {
        public IEnumerable<SearchFilter> SearchFilters { get; set; }

        public string OrderBy { get; set; }

        public bool IsDescendingOrder { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }

    [TypeScriptInterface]
    public class SearchFilter
    {
        public string PropertyName { get; set; }

        public object FilterValue { get; set; }
    }
}