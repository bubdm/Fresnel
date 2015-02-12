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
    public class SetPropertyCommand : ICommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private Core.Commands.SetPropertyCommand _SetPropertyCommand;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public SetPropertyCommand
            (
            Core.Commands.SetPropertyCommand setPropertyCommand,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _SetPropertyCommand = setPropertyCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(SetPropertyRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oProp = oObject.Properties[request.PropertyName];

                var oValue = (request.ReferenceValueId != Guid.Empty) ?
                            _ObserverCache.GetObserverById(request.ReferenceValueId) :
                            _ObserverCache.GetValueObserver(request.NonReferenceValue.ToStringOrNull(), oProp.Template.PropertyType);

                var previousValue = oProp.PreviousValue;
                _SetPropertyCommand.Invoke(oProp, oValue);

                // Other objects may have been affected by this property's value:
                _ObserverCache.ScanForChanges();

                // Done:
                var modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt);
                var thisPropertyChange = modifications.PropertyChanges.SingleOrDefault(p => p.ObjectId == oObject.ID && p.PropertyName == oProp.Template.Name);

                var infoText = thisPropertyChange == null ?
                                "Nothing was changed" :
                                request.NonReferenceValue == null ?
                                string.Concat(oProp.Template.FriendlyName, " was cleared") :
                                string.Concat(oProp.Template.FriendlyName, " changed to ", thisPropertyChange.State.FriendlyValue);

                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = infoText
                };
                return new GenericResponse()
                {
                    Passed = true,
                    Modifications = modifications,
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