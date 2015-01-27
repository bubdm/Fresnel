using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionRequest
    {
        public Guid CollectionID { get; set; }

        public Guid ElementID { get; set; }

        public string ElementTypeName { get; set; }
    }
}