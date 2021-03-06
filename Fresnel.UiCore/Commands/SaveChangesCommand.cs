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
        private ObserverRetriever _ObserverRetriever;
        private Core.Commands.SaveObjectCommand _SaveObjectCommand;
        private AbstractObjectVmBuilder _ObjectVmBuilder;
        private ModificationsVmBuilder _ModificationsVmBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SaveChangesCommand
            (
            ObserverRetriever observerRetriever,
            Core.Commands.SaveObjectCommand saveObjectCommand,
            AbstractObjectVmBuilder objectVmBuilder,
            ModificationsVmBuilder modificationsVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverRetriever = observerRetriever;
            _SaveObjectCommand = saveObjectCommand;
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

                var oObject = _ObserverRetriever.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var saveResult = _SaveObjectCommand.Invoke(oObject);
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
                Modifications = _ModificationsVmBuilder.BuildFrom(_ObserverRetriever.GetAllObservers(), startedAt)
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