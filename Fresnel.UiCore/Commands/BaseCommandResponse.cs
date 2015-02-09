using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using T4TS;
using System.ComponentModel;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public abstract class BaseCommandResponse
    {
        [DefaultValue(false)]
        public bool Passed { get; set; }

        [DefaultValue(false)]
        public bool Failed { get; set; }

        public IEnumerable<MessageVM> Messages { get; set; }

        /// <summary>
        /// Any modifications to other Objects that occurred as part of the operation
        /// </summary>
        public ModificationsVM Modifications { get; set; }
    }
}