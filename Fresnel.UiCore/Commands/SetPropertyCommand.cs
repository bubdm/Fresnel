using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Proxies;
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
        private ProxyCache _ProxyCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Introspection.Commands.SetPropertyCommand _SetPropertyCommand;
        private ModificationsBuilder _ModificationsBuilder;
        private IClock _Clock;

        public SetPropertyCommand
            (
            Introspection.Commands.SetPropertyCommand setPropertyCommand,
            ProxyCache proxyCache,
            AbstractObjectVMBuilder objectVMBuilder,
            ModificationsBuilder modificationsBuilder,
            IClock clock
        )
        {
            _SetPropertyCommand = setPropertyCommand;
            _ProxyCache = proxyCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public SetPropertyResult Invoke(SetPropertyRequest request)
        {
            try
            {
                var startedAt = Environment.TickCount;

                var proxy = _ProxyCache.GetProxyById(request.ObjectID);
                var oObject = ((IFresnelProxy)proxy).Meta;

                if (oObject != null)
                {
                    var oProp = oObject.Properties[request.PropertyName];

                    object newValue = null;
                    if (request.ReferenceValueId != Guid.Empty)
                    {
                        newValue = _ProxyCache.GetProxyById(request.ReferenceValueId);
                    }
                    else
                    {
                        newValue = request.NonReferenceValue;
                    }

                    _SetPropertyCommand.Invoke(proxy, request.PropertyName, newValue);
                }

                var proxyState = (IProxyState)proxy;

                return new SetPropertyResult()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(proxyState.ChangeLog, startedAt)
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
