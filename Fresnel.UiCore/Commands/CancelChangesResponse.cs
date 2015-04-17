using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CancelChangesResponse : BaseCommandResponse
    {
        public ObjectVM[] CancelledObjects { get; set; }
    }
}