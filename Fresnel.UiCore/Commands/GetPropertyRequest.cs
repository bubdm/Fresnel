using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetPropertyRequest
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }
    }
}