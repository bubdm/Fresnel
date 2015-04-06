using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class ExceptionMessagesBuilder
    {
        private IClock _Clock;

        public ExceptionMessagesBuilder(IClock clock)
        {
            _Clock = clock;
        }

        public MessageVM[] BuildFrom(Exception ex)
        {
            var exceptions = ex.FlattenAll();
            return this.BuildFrom(exceptions);
        }

        public MessageVM[] BuildFrom(ActionResult actionResult)
        {
            var exceptions = actionResult.FailureException.FlattenAll();
            return this.BuildFrom(exceptions);
        }

        public MessageVM[] BuildFrom(IEnumerable<Exception> exceptions)
        {
            var errorVMs = new List<MessageVM>();
            foreach (var ex in exceptions)
            {
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = ex.Message,
                    Detail = ex.ToString(),
                };

                errorVMs.Add(errorVM);
            }

            return errorVMs.ToArray();
        }

    }
}