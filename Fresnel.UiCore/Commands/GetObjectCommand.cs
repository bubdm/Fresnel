using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetObjectCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public GetObjectCommand
            (
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public GetPropertyResponse Invoke(GetObjectRequest request)
        {
            try
            {
                ObjectVM result = null;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                result = _ObjectVMBuilder.BuildFor(oObject);

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