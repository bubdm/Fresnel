using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SearchVM : BaseViewModel
    {

        public string SearchType { get; set; }

        public IEnumerable<PropertyVM> Properties { get; set; }

        public IEnumerable<string> SearchFilters { get; set; }

        public IEnumerable<string> OrderBy { get; set; }

        public IEnumerable<SearchResultItemVM> SearchResults { get; set; }

        public IEnumerable<SearchResultItemVM> SelectedItems { get; set; }

    }
}