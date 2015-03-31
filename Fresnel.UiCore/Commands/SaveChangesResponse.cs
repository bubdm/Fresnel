using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SaveChangesResponse : BaseCommandResponse
    {
        public ObjectVM[] SavedObjects { get; set; }
    }
}