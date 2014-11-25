using Castle.DynamicProxy;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{

    public class FinalTargetInterceptor : IInterceptor
    {
        
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (invocation.ReturnValue == invocation.InvocationTarget)
            {
                invocation.ReturnValue = invocation.Proxy;
            }
        }

    }

}
