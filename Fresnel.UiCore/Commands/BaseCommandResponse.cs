using Envivo.Fresnel.UiCore.Changes;
using Envivo.Fresnel.UiCore.Messages;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public abstract class BaseCommandResponse
    {
        public bool Passed { get; set; }

        public bool Failed { get; set; }

        public IEnumerable<MessageVM> Messages { get; set; }

        /// <summary>
        /// Any modifications to other Objects that occurred as part of the operation
        /// </summary>
        public Modifications Modifications { get; set; }
    }
}