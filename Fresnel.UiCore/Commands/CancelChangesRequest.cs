using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CancelChangesRequest
    {
        public Guid ParentObjectID { get; set; }

        public string ParentPropertyName { get; set; }

        /// <summary>
        /// The ID of the Object being cancelled
        /// </summary>
        public Guid ObjectID { get; set; }

    }
}