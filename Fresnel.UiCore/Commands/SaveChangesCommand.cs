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
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SaveChangesCommand
            (
            ObserverCache observerCache,
            SaveObjectCommand saveObjectCommand,
            AbstractObjectVmBuilder objectVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _SaveObjectCommand = saveObjectCommand;
            _ObjectVmBuilder = objectVmBuilder;
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

                var saveAction = _SaveObjectCommand.Invoke(oObject);
                if (saveAction.Failed)
                {
                    throw saveAction.FailureException;
                }

                var savedObservers = saveAction.Result;
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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new SaveChangesResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}