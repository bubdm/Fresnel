using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CreateAndSetPropertyRequest
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }

        /// <summary>
        /// The Type of the object to be created
        /// </summary>
        public string ClassTypeName { get; set; }

    }
}