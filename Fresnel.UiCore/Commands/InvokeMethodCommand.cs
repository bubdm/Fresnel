using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Changes;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeMethodCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Core.Commands.InvokeMethodCommand _InvokeMethodCommand;
        private ModificationsBuilder _ModificationsBuilder;
        private IClock _Clock;

        public InvokeMethodCommand
            (
            ObserverCache observerCache,
            Core.Commands.InvokeMethodCommand invokeMethodCommand,
            AbstractObjectVMBuilder objectVMBuilder,
            ModificationsBuilder modificationsBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _InvokeMethodCommand = invokeMethodCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public InvokeMethodResponse Invoke(InvokeMethodRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oMethod = oObject.Methods[request.MethodName];
                var oMethodResult = _InvokeMethodCommand.Invoke(oMethod, oObject.RealObject);
                var oResultObject = oMethodResult as ObjectObserver;

                var resultVM = oResultObject != null ?
                                _ObjectVMBuilder.BuildFor(oResultObject) :
                                null;

                _ObserverCache.ScanForChanges();

                // Done:
                var messages = new List<MessageVM>();
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("'", oMethod.Template.FriendlyName, "' completed.")
                };
                messages.Add(infoVM);

                if (oMethodResult != null && oMethodResult.RealObject != null)
                {
                    var resultMessageVM = new MessageVM()
                    {
                        IsInfo = true,
                        OccurredAt = _Clock.Now,
                        Text = string.Concat("Result: ", oMethodResult.RealObject)
                    };
                    messages.Add(resultMessageVM);
                }

                return new InvokeMethodResponse()
                {
                    Passed = true,
                    ResultObject = resultVM,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
                    Messages = messages.ToArray(),
                };
            }
            catch (Exception ex)
            {
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("'", request.MethodName, "' failed: ", ex.Message),
                    Detail = ex.StackTrace,
                };

                return new InvokeMethodResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}