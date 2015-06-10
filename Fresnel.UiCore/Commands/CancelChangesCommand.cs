using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CancelChangesCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private OuterObjectsIdentifier _OuterObjectsIdentifier;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private Core.Commands.CancelChangesCommand _CancelChangesCommand;
        private IPersistenceService _PersistenceService;
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private AbstractObjectVmBuilder _ObjectVmBuilder;
        private ModificationsVmBuilder _ModificationsVmBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CancelChangesCommand
            (
            ObserverRetriever observerRetriever,
            OuterObjectsIdentifier outerObjectsIdentifier,
            Core.Commands.GetPropertyCommand getPropertyCommand,
            Core.Commands.CancelChangesCommand cancelChangesCommand,
            IPersistenceService persistenceService,
            DirtyObjectNotifier dirtyObjectNotifier,
            AbstractObjectVmBuilder objectVmBuilder,
            ModificationsVmBuilder modificationsVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverRetriever = observerRetriever;
            _OuterObjectsIdentifier = outerObjectsIdentifier;
            _GetPropertyCommand = getPropertyCommand;
            _CancelChangesCommand = cancelChangesCommand;
            _PersistenceService = persistenceService;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _ObjectVmBuilder = objectVmBuilder;
            _ModificationsVmBuilder = modificationsVmBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public CancelChangesResponse Invoke(CancelChangesRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                ObjectObserver oObjectToCancel = null;
                if (request.PropertyName.IsNotEmpty())
                {
                    var oParent = this.GetObserver(request.ObjectID);
                    oObjectToCancel = this.GetObserver(oParent, request.PropertyName);
                    this.UndoChangesTo(oParent, request.PropertyName);
                }
                else
                {
                    oObjectToCancel = this.GetObserver(request.ObjectID);
                    this.UndoChangesTo(oObjectToCancel);
                }

                var cancelledObjectVMs = new List<ObjectVM>();
                var oCollection = oObjectToCancel as CollectionObserver;
                if (oCollection != null)
                {
                    cancelledObjectVMs.AddRange(this.ResetDirtyCollectionModifications(oCollection));
                }

                // Now reset the dirty flag up the chain:
                cancelledObjectVMs.AddRange(this.ResetDirtyFlags(oObjectToCancel));

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = "Cancelled changes to " + oObjectToCancel.Template.FriendlyName
                };
                return new CancelChangesResponse()
                {
                    Passed = true,
                    CancelledObjects = cancelledObjectVMs.ToArray(),
                    Messages = new MessageVM[] { infoVM },
                    Modifications = _ModificationsVmBuilder.BuildFrom(_ObserverRetriever.GetAllObservers(), startedAt)
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new CancelChangesResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

        private ObjectObserver GetObserver(Guid objectID)
        {
            var oObject = (ObjectObserver)_ObserverRetriever.GetObserverById(objectID);
            if (oObject == null)
                throw new UiCoreException("Cannot find object for " + objectID);
            return oObject;
        }

        private ObjectObserver GetObserver(ObjectObserver oParent, string propertyName)
        {
            var oProp = oParent.Properties[propertyName];
            var oResult = (CollectionObserver)_GetPropertyCommand.Invoke(oProp);
            if (oResult == null)
                throw new UiCoreException("Cannot find content for " + oProp.Template.Name);
            return oResult;
        }

        private void UndoChangesTo(ObjectObserver oObject)
        {
            _CancelChangesCommand.Invoke(oObject);

            if (oObject.ChangeTracker.IsPersistent)
            {
                _PersistenceService.Refresh(oObject.RealObject);
            }

            foreach (var oProp in oObject.Properties.Values)
            {
                var oObjectProp = oProp as ObjectPropertyObserver;
                if (oObjectProp != null)
                {
                    oObjectProp.IsLazyLoaded = false;
                }
            }
        }

        private void UndoChangesTo(ObjectObserver oObject, string propertyName)
        {
            var oCollection = this.GetObserver(oObject, propertyName);
            _CancelChangesCommand.Invoke(oCollection);

            var oObjectProp = oObject.Properties[propertyName] as ObjectPropertyObserver;
            if (oObjectProp != null)
            {
                _PersistenceService.LoadProperty(oObject.RealObject, propertyName);
                oObjectProp.IsLazyLoaded = false;
            }
        }

        private ObjectVM[] ResetDirtyCollectionModifications(CollectionObserver oCollection)
        {
            var affectedObjects = new List<ObjectObserver>();

            var addedItems = oCollection.ChangeTracker.AddedItems.ToArray();
            foreach (var item in addedItems)
            {
                var oItem = (ObjectObserver)_ObserverRetriever.GetObserver(item.Element);
                _DirtyObjectNotifier.ObjectIsNoLongerDirty(oItem);
                affectedObjects.Add(oItem);
            }

            var removedItems = oCollection.ChangeTracker.RemovedItems.ToArray();
            foreach (var item in removedItems)
            {
                var oItem = (ObjectObserver)_ObserverRetriever.GetObserver(item.Element);
                _DirtyObjectNotifier.ObjectIsNoLongerDirty(oItem);
                affectedObjects.Add(oItem);
            }

            var affectedObjectVMs = affectedObjects.Select(o => _ObjectVmBuilder.BuildFor(o)).ToArray();
            return affectedObjectVMs;
        }

        private ObjectVM[] ResetDirtyFlags(ObjectObserver oObject)
        {
            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oObject, 99);
            var affectedObjects = outerObjects
                                    .Where(o => o.Template.IsTrackable)
                                    .Where(o => o.ChangeTracker.IsDirty || o.ChangeTracker.HasDirtyObjectGraph)
                                    .ToList();
            affectedObjects.Add(oObject);

            _DirtyObjectNotifier.ObjectIsNoLongerDirty(oObject);

            var affectedObjectVMs = affectedObjects.Select(o => _ObjectVmBuilder.BuildFor(o)).ToArray();
            return affectedObjectVMs;
        }

    }
}