﻿using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SearchResultsVM : CollectionVM
    {
        public bool IsSearchResults { get { return true; } }
    }
}