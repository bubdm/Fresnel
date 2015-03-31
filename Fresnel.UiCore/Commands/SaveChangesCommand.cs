using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SaveChangesCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private SaveObjectCommand _SaveObjectCommand;
        private AbstractObjectVmBuilder _ObjectVmBuilder;
        private IClock _Clock;

        public SaveChangesCommand
            (
            ObserverCache observerCache,
            SaveObjectCommand saveObjectCommand,
            AbstractObjectVmBuilder objectVmBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _SaveObjectCommand = saveObjectCommand;
            _ObjectVmBuilder = objectVmBuilder;
            _Clock = clock;
        }

        public SaveChangesResponse Invoke(SaveChangesRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var savedObservers = _SaveObjectCommand.Invoke(oObject);
                var savedItemCount = savedObservers.Count();

                var savedObjectVMs = savedObservers.Select(o => _ObjectVmBuilder.BuildFor(o)).ToArray();

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = savedItemCount > 0 ?
                           string.Concat("Saved all changes (", savedItemCount, " items in total)") :
                           "Nothing was saved (did anything change?)"
                };
                return new SaveChangesResponse()
                {
                    Passed = savedItemCount > 0,
                    SavedObjects = savedObjectVMs,
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

                return new SaveChangesResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}