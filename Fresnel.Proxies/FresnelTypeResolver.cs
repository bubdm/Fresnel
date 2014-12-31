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
            var type = proxy.GetType();

            var castleProxy = proxy as IProxyTargetAccessor;
            if (castleProxy != null)
            {
                type = castleProxy.DynProxyGetTarget().GetType();
            }

            while (type.Assembly.IsDynamic)
            {
                type = type.BaseType;
            }

            if (type != typeof(object))
                return type;

            return null;
        }
    }
}
