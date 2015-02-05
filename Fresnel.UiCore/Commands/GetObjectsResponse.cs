using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetObjectsResponse : BaseCommandResponse
    {
        public CollectionVM Matches { get; set; }
    }
}