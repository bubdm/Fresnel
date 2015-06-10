using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private ModificationsVmBuilder _ModificationsVmBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public GetPropertyCommand
            (
            Core.Commands.GetPropertyCommand getPropertyCommand,
            ObserverRetriever observerRetriever,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsVmBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _GetPropertyCommand = getPropertyCommand;
            _ObserverRetriever = observerRetriever;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsVmBuilder = modificationsVmBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public GetPropertyResponse Invoke(GetPropertyRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                ObjectVM result = null;

                var oObject = _ObserverRetriever.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oProp = oObject.Properties[request.PropertyName];
                var oObjectProp = oProp as ObjectPropertyObserver;
                var oReturnValue = _GetPropertyCommand.Invoke(oProp);

                if (oReturnValue != null)
                {
                    result = _ObjectVMBuilder.BuildFor(oReturnValue);
                }

                // The Property's lazy-state may have changed, so we need to send the change to the client:
                var modifications = oObjectProp != null ?
                                    _ModificationsVmBuilder.BuildFrom(oObjectProp) :
                                    null;

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Loaded property", oProp.Template.FriendlyName, " for ", oObject.Template.FriendlyName)
                };
                return new GetPropertyResponse()
                {
                    Passed = true,
                    ReturnValue = result,
                    Messages = new MessageVM[] { infoVM },
                    Modifications = modifications
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GetPropertyResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}