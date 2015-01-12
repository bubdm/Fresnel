using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Changes;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SetPropertyCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Core.Commands.SetPropertyCommand _SetPropertyCommand;
        private ModificationsBuilder _ModificationsBuilder;
        private IClock _Clock;

        public SetPropertyCommand
            (
            Core.Commands.SetPropertyCommand setPropertyCommand,
            ObserverCache observerCache,
            AbstractObjectVMBuilder objectVMBuilder,
            ModificationsBuilder modificationsBuilder,
            IClock clock
        )
        {
            _SetPropertyCommand = setPropertyCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public SetPropertyResult Invoke(SetPropertyRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                var oProp = oObject.Properties[request.PropertyName];
                var oValue = GetValueObserver(request, oProp);

                var previousValue = oProp.PreviousValue;
                _SetPropertyCommand.Invoke(oProp, oValue);

                if (oProp.Template.IsAutoProperty)
                {
                    // The property setter doesn't have any logic, so there shouldn't be any other changes in the object graph
                }
                else
                {
                    // Other objects may have changed due to the property's setter logic:
                    _ObserverCache.ScanForChanges();
                }

                // Done:
                var infoVM = new MessageVM()
                {
                    OccurredAt = _Clock.Now,
                    Text = string.Concat(oProp.Template.FriendlyName, " changed from ", previousValue, " to ", request.NonReferenceValue)
                };
                return new SetPropertyResult()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
                    Messages = new MessageSetVM(new MessageVM[] { infoVM }, null, null)
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new SetPropertyResult()
                {
                    Failed = true,
                    Messages = new MessageSetVM(null, null, new ErrorVM[] { errorVM })
                };
            }
        }

        private BaseObjectObserver GetValueObserver(SetPropertyRequest request, BasePropertyObserver oProp)
        {
            BaseObjectObserver oValue = null;
            if (request.ReferenceValueId != Guid.Empty)
            {
                oValue = _ObserverCache.GetObserverById(request.ReferenceValueId);
            }
            else if (oProp.Template.PropertyType.IsEnum)
            {
                var value = Enum.Parse(oProp.Template.PropertyType, request.NonReferenceValue.ToString(), true);
                oValue = _ObserverCache.GetObserver(value, oProp.Template.PropertyType);
            }
            else if (oProp.Template.IsNonReference &&
                     !oProp.Template.IsNullableType &&
                     request.NonReferenceValue == null)
            {
                throw new UiCoreException("The given value is not allowed");
            }
            else
            {
                var value = request.NonReferenceValue != null ?
                                Convert.ChangeType(request.NonReferenceValue, oProp.Template.PropertyType) :
                                null;
                oValue = _ObserverCache.GetObserver(value, oProp.Template.PropertyType);
            }
            return oValue;
        }

    }
}
