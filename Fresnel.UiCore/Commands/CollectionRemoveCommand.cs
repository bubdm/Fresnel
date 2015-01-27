using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Changes;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionRemoveCommand
    {
        private ObserverCache _ObserverCache;
        private RemoveFromCollectionCommand _RemoveFromCollectionCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public CollectionRemoveCommand
            (
            ObserverCache observerCache,
            RemoveFromCollectionCommand removeFromCollectionCommand,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _RemoveFromCollectionCommand = removeFromCollectionCommand;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(CollectionRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oCollection = (CollectionObserver)_ObserverCache.GetObserverById(request.CollectionID);
                if (oCollection == null)
                    throw new UiCoreException("Cannot find collection with ID " + request.CollectionID);

                var oObject = (ObjectObserver)_ObserverCache.GetObserverById(request.ElementID);
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ElementID);

                var oResult = _RemoveFromCollectionCommand.Invoke(oCollection, oObject);

                // Other objects may have been affected by the action:
                _ObserverCache.ScanForChanges();

                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Removed ", oObject.RealObject.ToString(), " from the collection")
                };
                return new GenericResponse()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
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