using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SetParameterCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.SetParameterCommand _SetParameterCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public SetParameterCommand
            (
            Core.Commands.SetParameterCommand setParameterCommand,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _SetParameterCommand = setParameterCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(SetParameterRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oMethod = oObject.Methods[request.MethodName];
                var oParam = oMethod.Parameters[request.ParameterName];

                var oValue = (request.ReferenceValueId != Guid.Empty) ?
                            _ObserverCache.GetObserverById(request.ReferenceValueId) :
                            (request.NonReferenceValue != null) ?
                            _ObserverCache.GetValueObserver(request.NonReferenceValue.ToStringOrNull(), oParam.Template.ParameterType) :
                            _ObserverCache.GetObserver(null, oParam.Template.ParameterType);

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
                    Passed = true,
                    Messages = new MessageVM[] { infoVM }
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

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }

    }
}