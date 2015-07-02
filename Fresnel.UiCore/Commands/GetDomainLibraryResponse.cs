using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Classes;
using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetDomainLibraryResponse : BaseCommandResponse
    {

        /// <summary>
        /// The Domain Classes within the application
        /// </summary>
        public Namespace[] DomainClasses { get; set; }

        /// <summary>
        /// The Domain Services within the application
        /// </summary>
        public Namespace[] DomainServices { get; set; }

    }
}