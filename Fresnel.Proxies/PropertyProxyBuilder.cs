using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Proxies.ChangeTracking;
using Envivo.Fresnel.Proxies.Interceptors;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Envivo.Fresnel.Proxies
{

    public class PropertyProxyBuilder
    {
        private ProxyGenerator _ProxyGenerator;
        private Func<BasePropertyObserver, PropertyProxyInjectorInterceptor> _PropertyProxyInjectorInterceptorFactory;

        private Type[] _ObjectProxyInterfaceList;

        public PropertyProxyBuilder
            (
            ProxyGenerator proxyGenerator,
            Func<BasePropertyObserver, PropertyProxyInjectorInterceptor> propertyProxyInjectorInterceptorFactory
            )
        {
            _ProxyGenerator = proxyGenerator;
            _PropertyProxyInjectorInterceptorFactory = propertyProxyInjectorInterceptorFactory;

            _ObjectProxyInterfaceList = new Type[] 
            {
                typeof(IPropertyProxy) 
            };
        }

        public ProxyCache ProxyCache { get; set; }

        public object BuildFor(BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;

            var interceptor = _PropertyProxyInjectorInterceptorFactory(oProp);

            var propertyValue = tProp.GetField(oProp.OuterObject.RealObject);

            var proxy = tProp.PropertyType.IsInterface ?
                            _ProxyGenerator.CreateInterfaceProxyWithTarget(tProp.PropertyType, _ObjectProxyInterfaceList, propertyValue, interceptor) :
                            _ProxyGenerator.CreateClassProxyWithTarget(tProp.PropertyType, _ObjectProxyInterfaceList, propertyValue, interceptor);

            return proxy;
        }

    }

}
