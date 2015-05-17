using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionRemoveCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private Core.Commands.RemoveFromCollectionCommand _RemoveFromCollectionCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CollectionRemoveCommand
            (
            ObserverCache observerCache,
            Core.Commands.GetPropertyCommand getPropertyCommand,
            Core.Commands.RemoveFromCollectionCommand removeFromCollectionCommand,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _GetPropertyCommand = getPropertyCommand;
            _RemoveFromCollectionCommand = removeFromCollectionCommand;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(CollectionRemoveRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oParent = this.GetObserver(request.ParentObjectID);
                var oProp = (ObjectPropertyObserver)oParent.Properties[request.CollectionPropertyName];
                var oCollection = this.GetCollectionObserver(oParent, oProp);
                var oObject = this.GetObserver(request.ElementID);

                var result = _RemoveFromCollectionCommand.Invoke(oProp, oCollection, oObject);
                if (result == false)
                {
                    throw new UiCoreException("Unable to remove item from collection");
                }

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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

        private ObjectObserver GetObserver(Guid objectID)
        {
            var oObject = (ObjectObserver)_ObserverCache.GetObserverById(objectID);
            if (oObject == null)
                throw new UiCoreException("Cannot find object for " + objectID);
            return oObject;
        }

        private CollectionObserver GetCollectionObserver(ObjectObserver oParent, ObjectPropertyObserver oProp)
        {
            var oCollection = (CollectionObserver)_GetPropertyCommand.Invoke(oProp);
            if (oCollection == null)
                throw new UiCoreException("Cannot find collection for " + oProp.Template.Name);
            return oCollection;
        }
    }
}