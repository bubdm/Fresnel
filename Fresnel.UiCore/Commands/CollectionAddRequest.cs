using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionAddRequest
    {
        public Guid CollectionID { get; set; }

        public Guid[] ElementIDs { get; set; }

    }
}