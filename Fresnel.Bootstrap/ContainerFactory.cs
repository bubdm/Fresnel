using Autofac;
using Autofac.Features.ResolveAnything;
using Envivo.Fresnel.Core.Proxies;
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

            return builder.Build();
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

            builder.RegisterType<Proxies.ProxyBuilder>().As<IProxyBuilder>().SingleInstance();
        }

    }
}
