using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionRemoveRequest
    {
        public Guid CollectionID { get; set; }

        public Guid ElementID { get; set; }
    }
}