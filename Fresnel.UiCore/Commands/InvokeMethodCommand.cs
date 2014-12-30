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
    public class InvokeMethodCommand
    {
        private ObserverCache _ObserverCache;
        private ProxyCache _ProxyCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Core.Commands.InvokeMethodCommand _InvokeMethodCommand;
        private ModificationsBuilder _ModificationsBuilder;
        private IClock _Clock;

        public InvokeMethodCommand
            (
            ObserverCache observerCache,
            Core.Commands.InvokeMethodCommand invokeMethodCommand,
            ProxyCache proxyCache,
            AbstractObjectVMBuilder objectVMBuilder,
            ModificationsBuilder modificationsBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _InvokeMethodCommand = invokeMethodCommand;
            _ProxyCache = proxyCache;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
        }

        public GetPropertyResult Invoke(Guid objectId, string methodName)
        {
            try
            {
                var startedAt = Environment.TickCount;

                ObjectVM result = null;

                var proxy = _ProxyCache.GetProxyById(objectId);
                var oObject = proxy.Meta as ObjectObserver;

                if (oObject != null)
                {
                    var oMethod = oObject.Methods[methodName];
                    var returnValue = _InvokeMethodCommand.Invoke(oMethod, proxy) as ObjectObserver;

                    if (returnValue != null)
                    {
                        // Make sure we cache the proxy for use later in the session:
                        _ProxyCache.GetProxy(returnValue.RealObject);
                        result = _ObjectVMBuilder.BuildFor(returnValue);
                    }
                }

                var proxyState = (IProxyState)proxy;

                return new GetPropertyResult()
                {
                    Passed = true,
                    ReturnValue = result,
                    Modifications = _ModificationsBuilder.BuildFrom(proxyState.ChangeLog, startedAt)
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new GetPropertyResult()
                {
                    Failed = true,
                    ErrorMessages = new ErrorVM[] { errorVM }
                };
            }
        }


    }
}
