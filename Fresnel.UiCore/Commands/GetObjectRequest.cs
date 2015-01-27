using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetObjectRequest
    {
        public Guid ObjectID { get; set; }
    }
}