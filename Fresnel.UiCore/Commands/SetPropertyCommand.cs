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

                if (oObject != null)
                {
                    var oProp = oObject.Properties[request.PropertyName];

                    BaseObjectObserver oValue = null;
                    if (request.ReferenceValueId != Guid.Empty)
                    {
                        oValue = _ObserverCache.GetObserverById(request.ReferenceValueId);
                    }
                    else
                    {
                        var value = Convert.ChangeType(request.NonReferenceValue, oProp.Template.PropertyType);
                        oValue = _ObserverCache.GetObserver(value, oProp.Template.PropertyType);
                    }

                    _SetPropertyCommand.Invoke(oProp, oValue);

                    _ObserverCache.ScanForChanges();
                }

                return new SetPropertyResult()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt)
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new SetPropertyResult()
                {
                    Failed = true,
                    ErrorMessages = new ErrorVM[] { errorVM }
                };
            }
        }


    }
}
