using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CreateObjectRequest
    {
        public Guid ParentObjectID { get; set; }

        public string ClassTypeName { get; set; }
    }
}