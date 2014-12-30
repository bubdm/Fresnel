﻿using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Proxies;
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
    public class GetPropertyCommand
    {
        //private ObserverCache _ObserverCache;
        private ProxyCache _ProxyCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Introspection.Commands.GetPropertyCommand _GetPropertyCommand;
        private IClock _Clock;

        public GetPropertyCommand
            (
            //ObserverCache observerCache,
            Introspection.Commands.GetPropertyCommand getPropertyCommand,
            ProxyCache proxyCache,
            AbstractObjectVMBuilder objectVMBuilder,
            IClock clock
        )
        {
            //_ObserverCache = observerCache;
            _GetPropertyCommand = getPropertyCommand;
            _ProxyCache = proxyCache;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public GetPropertyResult Invoke(Guid objectId, string propertyName)
        {
            try
            {
                ObjectVM result = null;

                var proxy = _ProxyCache.GetProxyById(objectId);
                var oObject = ((IFresnelProxy)proxy).Meta;

                if (oObject != null)
                {
                    var oProp = oObject.Properties[propertyName];
                    var returnValue = _GetPropertyCommand.Invoke(proxy, propertyName);

                    if (returnValue != null)
                    {
                        // Make sure we cache the proxy for use later in the session:
                        var returnValueProxy = _ProxyCache.GetProxy(returnValue);
                        var oReturnValue = ((IFresnelProxy)returnValueProxy).Meta;
                        result = _ObjectVMBuilder.BuildFor(oReturnValue);
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
