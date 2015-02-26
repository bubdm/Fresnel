using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SearchResultsVM : CollectionVM
    {
        public bool IsSearchResults { get { return true; } }

        public SearchRequest OriginalRequest { get; set; }

        public bool AreMoreAvailable { get; set; }

        public bool AllowMultiSelect { get; set; }

    }
}