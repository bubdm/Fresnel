using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Proxies;
using System;
using System.Diagnostics;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Core.Observers;

namespace Envivo.Fresnel.Proxies.Interceptors
{
    /// <summary>
    /// This interceptor allows consumers to retrieve the underlying Observer for a Proxy
    /// </summary>
    public class ProxyMetaInterceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name + " " + invocation.Method.Name);

            invocation.Proceed();
        }

    }

}
