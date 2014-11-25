using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Proxies;
using System;
using System.Diagnostics;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Proxies
{

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
            Debug.WriteLine(this.GetType().Name);

            this.ReplaceArgumentsWithProxies(invocation);

            invocation.Proceed();
        }

        private void ReplaceArgumentsWithProxies(IInvocation invocation)
        {
            if (invocation.Arguments.Length == 0)
                return;

            for (var i = 0; i < invocation.Arguments.Length; i++)
            {
                var arg = invocation.GetArgumentValue(i);
                if (arg == null)
                    continue;

                var argType = arg.GetType();
                if (argType.IsNonReference())
                    continue;

                var proxy = arg as IFresnelProxy;
                if (proxy != null)
                    continue;

                var check = _CanBeProxiedSpecification.IsSatisfiedBy(arg);
                if (check.Failed)
                    continue;

                proxy = (IFresnelProxy)this.ProxyCache.GetProxy(arg);
                invocation.SetArgumentValue(i, proxy);
            }
        }

        public void Dispose()
        {
            this.ProxyCache = null;
        }

    }

}
