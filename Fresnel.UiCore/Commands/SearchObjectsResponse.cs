using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchObjectsResponse : BaseCommandResponse
    {
        public SearchResultsVM Results { get; set; }
    }
}