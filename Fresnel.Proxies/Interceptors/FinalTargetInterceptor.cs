using Castle.DynamicProxy;

namespace Envivo.Fresnel.Proxies
{

    /// <summary>
    /// This interceptor MUST be placed last, as it prevents the call to the underlying method
    /// </summary>
    public class FinalTargetInterceptor : IInterceptor
    {
        private ProxyCache _ProxyCache;

        public FinalTargetInterceptor(ProxyCache proxyCache)
        {
            _ProxyCache = proxyCache;
        }

        public void Intercept(IInvocation invocation)
        {
            // Only let the invocation proceed if a previous Interceptor didn't handle it already:



        }

    }

}
