using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeObjectMethodCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private InvokeMethodCommand _InvokeMethodCommand;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public InvokeObjectMethodCommand
            (
            ObserverCache observerCache,
            InvokeMethodCommand invokeMethodCommand,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _InvokeMethodCommand = invokeMethodCommand;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
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

                var result = _InvokeMethodCommand.Invoke(request, oMethod, startedAt);
                return result;
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new InvokeMethodResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

    }
}