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
        private BasePropertyObserver _oProperty;
        private object _OriginalPropertyValue;

        public PropertyProxyInjectorInterceptor
        (
            BasePropertyObserver oProperty
        )
        {
            _oProperty = oProperty;

            _OriginalPropertyValue = oProperty.Template.GetField(oProperty.OuterObject.RealObject);
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name + " " + invocation.Method.Name);

            if (_OriginalPropertyValue != null)
            {
                // Swap this temporary proxy for full object proxy:
                var replacementProxy = this.ProxyCache.GetProxy(_OriginalPropertyValue);

                var oOuterObject = _oProperty.OuterObject;
                var tProp = _oProperty.Template;
                tProp.SetField(oOuterObject.RealObject, replacementProxy);

                // Now we need to redirect the invocation to the *new* proxy:
                //invocation.Proceed(); // <-- DO NOT DO THIS!!!
                invocation.ReturnValue = invocation.Method.Invoke(replacementProxy, invocation.Arguments);

                // This interceptor is no longer used, so allow it to be GCed:
                // (This also prevents this proxy from being re-triggered)
                _oProperty = null;
                _OriginalPropertyValue = null;
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
            _oProperty = null;
            _OriginalPropertyValue = null;
        }

    }

}
