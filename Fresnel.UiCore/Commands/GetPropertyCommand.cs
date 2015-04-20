using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public GetPropertyCommand
            (
            Core.Commands.GetPropertyCommand getPropertyCommand,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _GetPropertyCommand = getPropertyCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public GetPropertyResponse Invoke(GetPropertyRequest request)
        {
            try
            {
                ObjectVM result = null;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oProp = oObject.Properties[request.PropertyName];
                var oObjectProp = oProp as ObjectPropertyObserver;
                var oReturnValue = _GetPropertyCommand.Invoke(oProp);

                if (oReturnValue != null)
                {
                    result = _ObjectVMBuilder.BuildFor(oReturnValue);
                }

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
                    Messages = new MessageVM[] { infoVM }
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