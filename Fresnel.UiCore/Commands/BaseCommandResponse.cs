using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;

using Envivo.Fresnel.UiCore.Changes;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{

    public abstract class BaseCommandResponse
    {

        public bool Passed { get; set; }

        public bool Failed { get; set; }

        public MessageSetVM Messages { get; set; }
        
        /// <summary>
        /// Any modifications to other Objects that occurred as part of the operation
        /// </summary>
        public Modifications Modifications { get; set; }

    }

}
