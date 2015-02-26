using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateAndSetPropertyCommand : ICommand
    {
        private CreateCommand _CreateCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private IClock _Clock;

        public CreateAndSetPropertyCommand
            (
            CreateCommand createCommand,
            SetPropertyCommand setPropertyCommand,
            IClock clock
            )
        {
            _CreateCommand = createCommand;
            _SetPropertyCommand = setPropertyCommand;
            _Clock = clock;
        }

        public CreateAndSetPropertyResponse Invoke(CreateAndSetPropertyRequest request)
        {
            try
            {
                var createResponse = _CreateCommand.Invoke(request.ClassTypeName);
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
                    NewObject = createResponse.NewObject
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