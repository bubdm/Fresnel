using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.ChangeTracking;
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
    public class CancelChangesCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private OuterObjectsIdentifier _OuterObjectsIdentifier;
        private EventTimeLine _EventTimeLine;
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private AbstractObjectVmBuilder _ObjectVmBuilder;
        private ModificationsVmBuilder _ModificationsVmBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CancelChangesCommand
            (
            ObserverCache observerCache,
            OuterObjectsIdentifier outerObjectsIdentifier,
            EventTimeLine eventTimeLine,
            DirtyObjectNotifier dirtyObjectNotifier,
            AbstractObjectVmBuilder objectVmBuilder,
            ModificationsVmBuilder modificationsVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _OuterObjectsIdentifier = outerObjectsIdentifier;
            _EventTimeLine = eventTimeLine;
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

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                // Undo all changes to this object since the last Save event
                this.UndoChangesTo(oObject);

                // Now reset the dirty flag up the chain:
                var cancelledObjectVMs = this.ResetDirtyFlags(oObject);

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = "Cancelled changes to " + oObject.Template.FriendlyName
                };
                return new CancelChangesResponse()
                {
                    Passed = true,
                    CancelledObjects = cancelledObjectVMs,
                    Messages = new MessageVM[] { infoVM },
                    Modifications = _ModificationsVmBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt)
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

        private void UndoChangesTo(ObjectObserver oObject)
        {
            var earliestPoint = _EventTimeLine.LastOrDefault(e=> e is SaveObjectEvent &&
                                                                 e.AffectedObjects.Contains(oObject)) ??
                                _EventTimeLine.FirstOrDefault();

            _EventTimeLine.UndoChangesTo(oObject, earliestPoint);
        }

        private ObjectVM[] ResetDirtyFlags(ObjectObserver oObject)
        {
            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oObject, 99);
            var objectsToReset = outerObjects
                                    .Where(o => o.Template.IsTrackable)
                                    .Where(o => o.ChangeTracker.IsDirty || o.ChangeTracker.HasDirtyObjectGraph)
                                    .ToArray();

            _DirtyObjectNotifier.ObjectIsNoLongerDirty(oObject);

            var cancelledObjectVMs = objectsToReset.Select(o => _ObjectVmBuilder.BuildFor(o)).ToArray();
            return cancelledObjectVMs;
        }

    }
}