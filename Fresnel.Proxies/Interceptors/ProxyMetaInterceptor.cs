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
    public class ProxyMetaInterceptor : IInterceptor, IDisposable
    {
        private ObjectObserver _oObject;

        public ProxyMetaInterceptor(ObjectObserver oObject)
        {
            _oObject = oObject;
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);

            if (invocation.Method.Name == "get_Meta")
            {
                invocation.ReturnValue = _oObject;
            }
            else
            {
                invocation.Proceed();
            }
        }

        public void Dispose()
        {
            _oObject = null;
            this.ProxyCache = null;
        }

    }

}
