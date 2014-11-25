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
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Proxies.CanBeProxiedSpecification),
                typeof(Fresnel.Proxies.CollectionAddInterceptor),
                typeof(Fresnel.Proxies.CollectionRemoveInterceptor),
                typeof(Fresnel.Proxies.FinalTargetInterceptor),
                typeof(Fresnel.Proxies.MethodInvokeInterceptor),
                typeof(Fresnel.Proxies.NotifyCollectionChangedInterceptor),
                typeof(Fresnel.Proxies.NotifyPropertyChangedInterceptor),
                typeof(Fresnel.Proxies.PrimaryInterceptor),
                typeof(Fresnel.Proxies.PropertyGetInterceptor),
                typeof(Fresnel.Proxies.PropertySetInterceptor),
                typeof(Fresnel.Proxies.ProxyBuilder),
                typeof(Fresnel.Proxies.ProxyCache),

                typeof(Fresnel.Proxies.InterceptorSelector),
                typeof(Fresnel.Proxies.ProxyGenerationHook),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] { 
            };
        }

    }
}
