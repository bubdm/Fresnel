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
        private ObserverCache _ObserverCache;
        private CreateCommand _CreateCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public CreateAndSetPropertyCommand
            (
            ObserverCache observerCache,
            CreateCommand createCommand,
            SetPropertyCommand setPropertyCommand,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _ObserverCache = observerCache;
            _CreateCommand = createCommand;
            _SetPropertyCommand = setPropertyCommand;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public CreateAndSetPropertyResponse Invoke(CreateAndSetPropertyRequest request)
        {
            var startedAt = SequentialIdGenerator.Next;

            try
            {
                var createRequest = new CreateRequest()
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
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
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

                return new CreateAndSetPropertyResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}