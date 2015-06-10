using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateAndSetPropertyCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private CreateObjectCommand _CreateCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CreateAndSetPropertyCommand
            (
            ObserverRetriever observerRetriever,
            CreateObjectCommand createCommand,
            SetPropertyCommand setPropertyCommand,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _ObserverRetriever = observerRetriever;
            _CreateCommand = createCommand;
            _SetPropertyCommand = setPropertyCommand;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public CreateAndSetPropertyResponse Invoke(CreateAndSetPropertyRequest request)
        {
            var startedAt = SequentialIdGenerator.Next;

            try
            {
                var createRequest = new CreateObjectRequest()
                {
                     ParentObjectID = request.ObjectID,
                     ClassTypeName = request.ClassTypeName
                };
                var createResponse = _CreateCommand.Invoke(createRequest);
                if (createResponse.Failed)
                {
                    return new CreateAndSetPropertyResponse()
                    {
                        Failed = true,
                        Messages = createResponse.Messages,
                    };
                }

                var setRequest = new SetPropertyRequest()
                {
                    ObjectID = request.ObjectID,
                    PropertyName = request.PropertyName,
                    ReferenceValueId = createResponse.NewObject.ID
                };
                var setResponse = _SetPropertyCommand.Invoke(setRequest);
                if (setResponse.Failed)
                {
                    return new CreateAndSetPropertyResponse()
                    {
                        Failed = true,
                        Messages = createResponse.Messages,
                    };
                }

                return new CreateAndSetPropertyResponse()
                {
                    Passed = true,
                    NewObject = createResponse.NewObject,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverRetriever.GetAllObservers(), startedAt),
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new CreateAndSetPropertyResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}