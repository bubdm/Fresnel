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

        public CleanupSessionResult Invoke()
        {
            try
            {
                _ObserverCache.CleanUp();

                var infoVM = new MessageVM()
                {
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Your session is now clear")
                };
                return new CleanupSessionResult()
                {
                    Passed = true,
                    Messages = new MessageSetVM(new MessageVM[] { infoVM }, null, null)
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new CleanupSessionResult()
                {
                    Failed = true,
                    Messages = new MessageSetVM(null, null, new ErrorVM[] { errorVM })
                };
            }
        }


    }
}
