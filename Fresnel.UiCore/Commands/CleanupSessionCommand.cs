using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CleanupSessionCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private IPersistenceService _PersistenceService;
        private IClock _Clock;

        public CleanupSessionCommand
            (
            ObserverCache observerCache,
            IPersistenceService persistenceService,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _PersistenceService = persistenceService;
            _Clock = clock;
        }

        public GenericResponse Invoke()
        {
            try
            {
                _PersistenceService.RollbackChanges();
                _ObserverCache.CleanUp();

                var infoVM = new MessageVM()
                {
                    IsInfo = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Your session is now clear")
                };
                return new GenericResponse()
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

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}