using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace Envivo.Fresnel.Proxies
{

    public class FresnelTypeResolver : IRealTypeResolver
    {
        public Type GetRealType(object proxy)
        {
            var castleProxy = proxy as IProxyTargetAccessor;

            if (castleProxy != null)
            {
                return castleProxy.DynProxyGetTarget().GetType();
            }

            return null;
        }
    }
}
