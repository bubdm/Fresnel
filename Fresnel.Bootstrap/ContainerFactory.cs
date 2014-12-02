using Autofac;
using Autofac.Features.ResolveAnything;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap
{
    public class ContainerFactory
    {
        /// <summary>
        /// Returns an AutoFac IoC container
        /// </summary>
        /// <returns></returns>
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            this.RegisterMandatoryModules(builder);

            // THIS SEEMS TO BE VERY SLOW, HENCE IT IS DISABLED:
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var result = builder.Build();

            this.SetupContainer(result);

            return result;
        }

        private void SetupContainer(IContainer container)
        {
            var realTypeResolver = container.Resolve<RealTypeResolver>();
            var fresnelTypeResolver = container.Resolve<FresnelTypeResolver>();

            realTypeResolver.Register(fresnelTypeResolver);
        }

        /// <summary>
        /// Returns an AutoFac IoC container
        /// </summary>
        /// <param name="dependencyModules"></param>
        /// <returns></returns>
        public IContainer Build(IEnumerable<Module> dependencyModules)
        {
            var builder = new ContainerBuilder();
            this.RegisterMandatoryModules(builder);

            // Now bolt in the Consumer's dependencies:
            foreach (var module in dependencyModules)
            {
                builder.RegisterModule(module);
            }

            return builder.Build();
        }

        private void RegisterMandatoryModules(ContainerBuilder builder)
        {
            builder.RegisterModule<IntrospectionDependencies>();
            builder.RegisterModule<CoreDependencies>();
            builder.RegisterModule<ProxiesDependencies>();
            builder.RegisterModule<UiCoreDependencies>();

            builder.RegisterType<Proxies.ProxyBuilder>().As<IProxyBuilder>().SingleInstance();
        }

    }
}
