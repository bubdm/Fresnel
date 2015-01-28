using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class CollectionElementVM
    {
        public Guid CollectionId { get; set; }

        public Guid ElementId { get; set; }
    }
}