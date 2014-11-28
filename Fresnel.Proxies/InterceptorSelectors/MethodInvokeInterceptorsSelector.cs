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

    public class MethodInvokeInterceptorsSelector : IInterceptorsSelector
    {
        public bool CanHandle(MethodInfo method)
        {
            return !method.IsSpecialName;
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            var result = new IInterceptor[]
            {
                allInterceptors.OfType<PrimaryInterceptor>().Single(),
                allInterceptors.OfType<MethodInvokeInterceptor>().Single(),
                allInterceptors.OfType<FinalTargetInterceptor>().Single(),
            };
            return result;
        }

    }

}
