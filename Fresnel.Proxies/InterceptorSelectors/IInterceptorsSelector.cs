using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{
    public interface IInterceptorsSelector
    {
        bool CanHandle(MethodInfo method);

        IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors);

    }
}
