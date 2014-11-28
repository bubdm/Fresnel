using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class ProxyMetaInterceptorsSelector : IInterceptorsSelector
    {

        public bool CanHandle(MethodInfo method)
        {
            return method.DeclaringType.Equals(typeof(IFresnelProxy));
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            return allInterceptors
                    .Where(i => i is ProxyMetaInterceptor)
                    .ToArray();
        }

    }

}
