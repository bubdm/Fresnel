using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SaveChangesRequest
    {
        public Guid ObjectID { get; set; }
    }
}