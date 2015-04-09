using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionRemoveRequest
    {
        public Guid ParentObjectID { get; set; }

        public string CollectionPropertyName { get; set; }

        public Guid ElementID { get; set; }
    }
}