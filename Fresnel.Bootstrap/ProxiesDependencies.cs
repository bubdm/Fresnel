using Autofac;
using Envivo.Fresnel.Core.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap
{
    public class ProxiesDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerDependencyInstanceTypes())
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerSessionInstanceTypes())
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Castle.DynamicProxy.ProxyGenerator),
                
                typeof(Fresnel.Proxies.CanBeProxiedSpecification),
                typeof(Fresnel.Proxies.FresnelTypeResolver),

                typeof(Fresnel.Proxies.Interceptors.CollectionAddInterceptor),
                typeof(Fresnel.Proxies.Interceptors.CollectionRemoveInterceptor),
                typeof(Fresnel.Proxies.Interceptors.FinalTargetInterceptor),
                typeof(Fresnel.Proxies.Interceptors.MethodInvokeInterceptor),
                typeof(Fresnel.Proxies.Interceptors.PrimaryInterceptor),
                typeof(Fresnel.Proxies.Interceptors.PropertyGetInterceptor),
                typeof(Fresnel.Proxies.Interceptors.PropertySetInterceptor),

                typeof(Fresnel.Proxies.Interceptors.IgnoreMethodInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.MethodInvokeInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.CollectionAddInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.CollectionRemoveInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.NotifyCollectionChangedInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.PropertyGetInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.PropertySetInterceptorsSelector),
                typeof(Fresnel.Proxies.Interceptors.PropertyProxyInjectorInterceptor),
                typeof(Fresnel.Proxies.Interceptors.NotifyPropertyChangedInterceptorsSelector),

                typeof(Fresnel.Proxies.Interceptors.InterceptorSelector),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Proxies.Interceptors.NotifyCollectionChangedInterceptor),
                typeof(Fresnel.Proxies.Interceptors.NotifyPropertyChangedInterceptor),
            };
        }

        private Type[] GetPerSessionInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Proxies.ProxyCache),
                typeof(Fresnel.Proxies.ProxyBuilder),
                typeof(Fresnel.Proxies.PropertyProxyBuilder),
                typeof(Fresnel.Proxies.ChangeTracking.ChangeLog),
            };
        }

    }
}
