using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Classes;
using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetDomainServicesResponse : BaseCommandResponse
    {

        public IEnumerable<Namespace> Namespaces { get; set; }

    }
}