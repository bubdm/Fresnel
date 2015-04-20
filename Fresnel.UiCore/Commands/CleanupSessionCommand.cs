using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CleanupSessionCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private IPersistenceService _PersistenceService;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CleanupSessionCommand
            (
            ObserverCache observerCache,
            IPersistenceService persistenceService,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _PersistenceService = persistenceService;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}