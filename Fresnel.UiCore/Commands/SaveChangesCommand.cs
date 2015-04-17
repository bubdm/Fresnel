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
        private Func<ObjectObserver, SaveObjectEvent> _SaveObjectEventFactory;
        private EventTimeLine _EventTimeLine;
        private AbstractObjectVmBuilder _ObjectVmBuilder;
        private ModificationsVmBuilder _ModificationsVmBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SaveChangesCommand
            (
            ObserverCache observerCache,
            Func<ObjectObserver, SaveObjectEvent> saveObjectEventFactory,
            EventTimeLine eventTimeLine,
            AbstractObjectVmBuilder objectVmBuilder,
            ModificationsVmBuilder modificationsVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _SaveObjectEventFactory = saveObjectEventFactory;
            _EventTimeLine = eventTimeLine;
            _ObjectVmBuilder = objectVmBuilder;
            _ModificationsVmBuilder = modificationsVmBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
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

                var saveEvent = _SaveObjectEventFactory(oObject);
                _EventTimeLine.Add(saveEvent);

                var saveResult = (ActionResult<ObjectObserver[]>)saveEvent.Do();
                if (saveResult.Failed)
                {
                    throw saveResult.FailureException;
                }

                var savedObservers = saveResult.Result;
                var savedItemCount = savedObservers.Count();
                var savedObjectVMs = savedObservers.Select(o => _ObjectVmBuilder.BuildFor(o)).ToArray();

                // Done:
                return (savedItemCount == 0) ?
                        this.CreateWarningResponse() :
                        this.CreateSuccessResponse(startedAt, savedItemCount, savedObjectVMs);
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new SaveChangesResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

        private SaveChangesResponse CreateSuccessResponse(long startedAt, int savedItemCount, ObjectVM[] savedObjectVMs)
        {
            var infoVM = new MessageVM()
            {
                IsSuccess = true,
                OccurredAt = _Clock.Now,
                Text = string.Concat("Saved all changes (", savedItemCount, " items in total)")
            };
            return new SaveChangesResponse()
            {
                Passed = true,
                SavedObjects = savedObjectVMs,
                Messages = new MessageVM[] { infoVM },
                Modifications = _ModificationsVmBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt)
            };
        }

        private SaveChangesResponse CreateWarningResponse()
        {
            var warningVM = new MessageVM()
            {
                IsWarning = true,
                OccurredAt = _Clock.Now,
                Text = "Nothing was saved (did anything change?)"
            };
            return new SaveChangesResponse()
            {
                Failed = true,
                Messages = new MessageVM[] { warningVM },
            };
        }

    }
}