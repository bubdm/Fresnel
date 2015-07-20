using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeMethodCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private DomainServiceObserverRetriever _DomainServiceObserverRetriever;
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.InvokeMethodCommand _InvokeMethodCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public InvokeMethodCommand
            (
            TemplateCache templateCache,
            ObserverRetriever observerRetriever,
            DomainServiceObserverRetriever domainServiceObserverRetriever,
            ObserverCache observerCache,
            Core.Commands.InvokeMethodCommand invokeMethodCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _ObserverRetriever = observerRetriever;
            _DomainServiceObserverRetriever = domainServiceObserverRetriever;
            _ObserverCache = observerCache;
            _InvokeMethodCommand = invokeMethodCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }


        public InvokeMethodResponse Invoke(InvokeMethodRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = this.DetermineObserver(request);
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oMethod = oObject.Methods[request.MethodName];

                this.UpdateParameters(oMethod, request);

                var oMethodResult = _InvokeMethodCommand.Invoke(oMethod, oMethod.OuterObject.RealObject);
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
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverRetriever.GetAllObservers(), startedAt),
                    Messages = messages.ToArray(),
                };
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

        private ObjectObserver DetermineObserver(InvokeMethodRequest request)
        {
            var result = request.ObjectID.HasValue ?
                            _ObserverRetriever.GetObserverById(request.ObjectID.Value) :
                            _DomainServiceObserverRetriever.GetObserver(request.ClassFullTypeName);
            return result;
        }

        private void UpdateParameters(MethodObserver oMethod, InvokeMethodRequest request)
        {
            if (request.Parameters == null ||
                !request.Parameters.Any())
                return;

            foreach (var paramVM in request.Parameters)
            {
                var oParam = oMethod.Parameters[paramVM.InternalName];

                var oValue = (paramVM.State.ReferenceValueID != null) ?
                            _ObserverRetriever.GetObserverById(paramVM.State.ReferenceValueID.Value) :
                            _ObserverRetriever.GetValueObserver(paramVM.State.Value.ToStringOrNull(), oParam.Template.ParameterType);

                oParam.Value = oValue.RealObject;
            }
        }

    }
}