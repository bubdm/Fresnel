using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CancelChangesRequest
    {
        public Guid ObjectID { get; set; }
    }
}