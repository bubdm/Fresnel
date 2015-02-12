﻿using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchObjectsRequest
    {
        public string SearchType { get; set; }

        public IEnumerable<string> SearchFilters { get; set; }

        public IEnumerable<string> OrderBy { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}