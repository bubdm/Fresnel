using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using Envivo.Fresnel.Introspection;
using System.Collections.Generic;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SetParameterCommand : ICommand
    {
        private ObserverRetriever _ObserverRetriever;
        private DomainServiceObserverRetriever _DomainServiceObserverRetriever;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.SetParameterCommand _SetParameterCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SetParameterCommand
            (
            Core.Commands.SetParameterCommand setParameterCommand,
            ObserverRetriever observerRetriever,
            DomainServiceObserverRetriever domainServiceObserverRetriever,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _SetParameterCommand = setParameterCommand;
            _ObserverRetriever = observerRetriever;
            _DomainServiceObserverRetriever = domainServiceObserverRetriever;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(SetParameterRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = this.DetermineObserver(request);
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oMethod = oObject.Methods[request.MethodName];
                var oParam = oMethod.Parameters[request.ParameterName];

                var oValue = (request.ReferenceValueId != Guid.Empty) ?
                            _ObserverRetriever.GetObserverById(request.ReferenceValueId) :
                            (request.NonReferenceValue != null) ?
                            _ObserverRetriever.GetValueObserver(request.NonReferenceValue.ToStringOrNull(), oParam.Template.ParameterType) :
                            _ObserverRetriever.GetObserver(null, oParam.Template.ParameterType);

                if (oParam.Template.IsNonReference &&
                    !oParam.Template.IsNullableType &&
                    request.NonReferenceValue == null)
                {
                    throw new UiCoreException("Please provide a valid value for " + oParam.Template.FriendlyName);
                }

                _SetParameterCommand.Invoke(oParam, oValue);

                // Done:
                var friendlyName = oParam.Template.FriendlyName;
                var infoText = request.ReferenceValueId != Guid.Empty ?
                                string.Concat(friendlyName, " changed to ", oValue.RealObject.ToStringOrNull()) :
                                request.NonReferenceValue == null ?
                                string.Concat(friendlyName, " was cleared") :
                                string.Concat(friendlyName, " changed to ", oValue.RealObject.ToStringOrNull());

                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = infoText
                };
                return new GenericResponse()
                {
                    Modifications = _ModificationsBuilder.BuildFrom(oMethod, oParam),
                    Passed = true,
                    Messages = new MessageVM[] { infoVM }
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

        private ObjectObserver DetermineObserver(SetParameterRequest request)
        {
            var result = request.ObjectID.HasValue ?
                            _ObserverRetriever.GetObserverById(request.ObjectID.Value) :
                            _DomainServiceObserverRetriever.GetObserver(request.ClassFullTypeName);
            return result;
        }
    }
}