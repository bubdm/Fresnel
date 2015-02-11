using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetObjectsRequest
    {
        public string TypeName { get; set; }

        public string OrderBy { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}