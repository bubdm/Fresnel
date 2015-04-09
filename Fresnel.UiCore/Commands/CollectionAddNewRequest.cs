using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionAddNewRequest
    {
        public Guid ParentObjectID { get; set; }

        public string CollectionPropertyName { get; set; }

        public string ElementTypeName { get; set; }
    }
}