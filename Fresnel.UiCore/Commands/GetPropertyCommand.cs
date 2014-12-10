using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Proxies;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyCommand
    {
        private ObserverCache _ObserverCache;
        private ProxyCache _ProxyCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;

        public GetPropertyCommand
            (
            ObserverCache observerCache,
            Core.Commands.GetPropertyCommand getPropertyCommand,
            ProxyCache proxyCache,
            AbstractObjectVMBuilder objectVMBuilder
            )
        {
            _ObserverCache = observerCache;
            _GetPropertyCommand = getPropertyCommand;
            _ProxyCache = proxyCache;
            _ObjectVMBuilder = objectVMBuilder;
        }

        public GetPropertyResult Invoke(Guid objectId, string propertyName)
        {
            try
            {
                ObjectVM result = null;

                var oObject = _ObserverCache.GetObserverById(objectId) as ObjectObserver;

                if (oObject != null)
                {
                    var oProp = oObject.Properties[propertyName];
                    var returnValue = _GetPropertyCommand.Invoke(oProp) as ObjectObserver;

                    if (returnValue != null)
                    {
                        // Make sure we cache the proxy for use later in the session:
                        var proxy = _ProxyCache.GetProxy(returnValue.RealObject);
                        result = _ObjectVMBuilder.BuildFor(returnValue);
                    }
                }
                
                return new GetPropertyResult()
                {
                    Passed = true,
                    ReturnValue = result
                };
            }
            catch (Exception ex)
            {
                return new GetPropertyResult()
                {
                    Failed = true,
                    ErrorMessages = new string[] { ex.Message }
                };
            }
        }


    }
}
