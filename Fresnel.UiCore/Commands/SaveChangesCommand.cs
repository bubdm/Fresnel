using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SaveChangesCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private IPersistenceService _PersistenceService;
        private IClock _Clock;

        public SaveChangesCommand
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

        public GenericResponse Invoke(SaveChangesRequest request)
        {
            try
            {
                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var savedItems = _PersistenceService.SaveChanges(oObject.RealObject);

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = savedItems > 0 ?
                           string.Concat("Saved all changes (", savedItems, " items in total)") :
                           "Nothing was saved (did anything change?)"
                };
                return new GenericResponse()
                {
                    Passed = savedItems > 0,
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