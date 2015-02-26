using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchParameterRequest : SearchRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

        public string ParameterName { get; set; }

    }
}