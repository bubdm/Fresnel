using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Proxies;
using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{

    /// <summary>
    /// This interceptor MUST be placed first, as it knows how to replace Proxies with actual Domain Objects
    /// </summary>
    public class PrimaryInterceptor : IInterceptor, IDisposable
    {
        private CanBeProxiedSpecification _CanBeProxiedSpecification;

        public PrimaryInterceptor(CanBeProxiedSpecification canBeProxiedSpecification)
        {
            _CanBeProxiedSpecification = canBeProxiedSpecification;
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(invocation.ToString());

            if (invocation.Method.Name == "get_Meta")
            {
                var observer = ((IFresnelProxy)invocation.Proxy).Meta;
                invocation.ReturnValue = observer;
            }
            else
            {
                this.ReplaceArgumentProxiesWithRealObjects(invocation);

                invocation.Proceed();
            }

            // If the result is an Object/Collection, replace it with an IFresnelProxy:
            this.ReplaceReturnValueWithProxy(invocation);
        }

        private void ReplaceArgumentProxiesWithRealObjects(IInvocation invocation)
        {
            if (invocation.Arguments.Length == 0)
                return;

            for (var i = 0; i < invocation.Arguments.Length; i++)
            {
                var arg = invocation.GetArgumentValue(i);

                var proxy = arg as IFresnelProxy;
                if (proxy != null)
                {
                    invocation.SetArgumentValue(i, proxy.Meta.RealObject);
                }
            }
        }

        private void ReplaceArgumentObjectsWithProxies(IInvocation invocation)
        {
            if (invocation.Arguments.Length == 0)
                return;

            for (var i = 0; i < invocation.Arguments.Length; i++)
            {
                var arg = invocation.GetArgumentValue(i);

                var check = _CanBeProxiedSpecification.IsSatisfiedBy(arg);
                if (check.Passed)
                {
                    invocation.SetArgumentValue(i, this.ProxyCache.GetProxy(arg));
                }
                else
                {
                    // Continue using the original argument value
                }
            }
        }

        private void ReplaceReturnValueWithProxy(IInvocation invocation)
        {
            var check = _CanBeProxiedSpecification.IsSatisfiedBy(invocation.ReturnValue);
            if (check.Passed)
            {
                invocation.ReturnValue = this.ProxyCache.GetProxy(invocation.ReturnValue);
            }
            else
            {
                // Continue using the original return value
            }
        }

        public void Dispose()
        {
            _CanBeProxiedSpecification = null;
            this.ProxyCache = null;
        }

    }

}
