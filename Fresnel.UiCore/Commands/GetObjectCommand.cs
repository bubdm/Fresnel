using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetObjectCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public GetObjectCommand
            (
            ObserverRetriever observerRetriever,
            AbstractObjectVmBuilder objectVMBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverRetriever = observerRetriever;
            _ObjectVMBuilder = objectVMBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public GetPropertyResponse Invoke(GetObjectRequest request)
        {
            try
            {
                var oObject = _ObserverRetriever.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var result = _ObjectVMBuilder.BuildFor(oObject);

                // Done:
                return new GetPropertyResponse()
                {
                    Passed = true,
                    ReturnValue = result
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