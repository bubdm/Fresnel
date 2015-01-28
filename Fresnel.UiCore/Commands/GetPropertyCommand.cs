using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private IClock _Clock;

        public GetPropertyCommand
            (
            Core.Commands.GetPropertyCommand getPropertyCommand,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            IClock clock
        )
        {
            _GetPropertyCommand = getPropertyCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
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
                return new GetPropertyResponse()
                {
                    Passed = true,
                    ReturnValue = result
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

                return new GetPropertyResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}