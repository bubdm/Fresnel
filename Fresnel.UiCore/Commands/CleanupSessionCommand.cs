using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CleanupSessionCommand
    {
        private ObserverCache _ObserverCache;
        private IClock _Clock;

        public CleanupSessionCommand
            (
            ObserverCache observerCache,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _Clock = clock;
        }

        public CleanupSessionResponse Invoke()
        {
            try
            {
                _ObserverCache.CleanUp();

                var infoVM = new MessageVM()
                {
                    IsInfo = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Your session is now clear")
                };
                return new CleanupSessionResponse()
                {
                    Passed = true,
                    Messages = new MessageVM[] { infoVM }
                };
            }
            catch (Exception ex)
            {
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = ex.Message,
                    Detail = ex.ToString(),
                };

                return new CleanupSessionResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }


    }
}
