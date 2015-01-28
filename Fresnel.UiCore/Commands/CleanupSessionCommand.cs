using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

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