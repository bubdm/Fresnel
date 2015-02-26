using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SearchPropertyRequest : SearchRequest
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }

    }
}