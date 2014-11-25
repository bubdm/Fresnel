using Castle.DynamicProxy;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{

    /// <summary>
    /// This interceptor MUST be placed last, as it prevents the call to the underlying method
    /// </summary>
    public class FinalTargetInterceptor : IInterceptor
    {
        private CanBeProxiedSpecification _CanBeProxiedSpecification;

        public FinalTargetInterceptor(CanBeProxiedSpecification canBeProxiedSpecification)
        {
            _CanBeProxiedSpecification = canBeProxiedSpecification;
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);

            invocation.Proceed();

            // If the result is an Object/Collection, replace it with an IFresnelProxy:
            this.ReplaceReturnValueWithProxy(invocation);
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

    }

}
