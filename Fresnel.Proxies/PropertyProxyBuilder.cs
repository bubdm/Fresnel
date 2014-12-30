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
        private PropertyProxyInjectorInterceptor _PropertyProxyInjectorInterceptor;

        private Type[] _ObjectProxyInterfaceList;

        public PropertyProxyBuilder
            (
            ProxyGenerator proxyGenerator,
            PropertyProxyInjectorInterceptor propertyProxyInjectorInterceptor
            )
        {
            _ProxyGenerator = proxyGenerator;
            _PropertyProxyInjectorInterceptor = propertyProxyInjectorInterceptor;

            _ObjectProxyInterfaceList = new Type[] 
            {
                typeof(IPropertyProxy) 
            };
        }

        public ProxyCache ProxyCache { get; set; }


        public object BuildFor(object targetObject, BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;

            // Try to use the field whenever possible, to prevent properties from triggering a lazy-load:
            var propertyValue = tProp.GetField(targetObject) ?? tProp.GetProperty(targetObject);

            var proxyState = new PropertyProxyState()
            {
                PropertyTemplate = oProp.Template,
                OuterObject = targetObject,
                OriginalPropertyValue = propertyValue
            };
            var proxyGenerationOptions = new ProxyGenerationOptions();
            proxyGenerationOptions.AddMixinInstance(proxyState);

            var proxy = tProp.PropertyType.IsInterface ?
                            _ProxyGenerator.CreateInterfaceProxyWithTarget(tProp.PropertyType, _ObjectProxyInterfaceList, propertyValue, proxyGenerationOptions, _PropertyProxyInjectorInterceptor) :
                            _ProxyGenerator.CreateClassProxyWithTarget(tProp.PropertyType, _ObjectProxyInterfaceList, propertyValue, proxyGenerationOptions, _PropertyProxyInjectorInterceptor);

            return proxy;
        }

    }

}
