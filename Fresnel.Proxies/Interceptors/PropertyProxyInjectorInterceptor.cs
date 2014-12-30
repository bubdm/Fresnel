using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class PropertyProxyInjectorInterceptor : IInterceptor
    {
        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name + " " + invocation.Method.Name);

            if (invocation.Method.DeclaringType == typeof(IPropertyProxy))
            {
                invocation.Proceed();
                return;
            }

            // The first time this property is accessed, it is swapped out for a proper proxy:
            var propertyProxy = invocation.Proxy as IPropertyProxy;
            if (propertyProxy != null)
            {
                // Swap this temporary proxy for full object proxy:
                var replacementProxy = this.ProxyCache.GetProxy(propertyProxy.OriginalPropertyValue);

                Debug.WriteLine("Swapping property proxy for a real one");
                propertyProxy.PropertyTemplate.SetField(propertyProxy.OuterObject, replacementProxy);

                // Now we need to redirect the invocation to the *new* proxy:
                //invocation.Proceed(); // <-- DO NOT DO THIS!!!
                invocation.ReturnValue = invocation.Method.Invoke(replacementProxy, invocation.Arguments);
            }
        }

        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public void Dispose()
        {
            this.ProxyCache = null;
        }

    }

}
