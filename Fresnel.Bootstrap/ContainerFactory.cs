using Autofac;
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
            builder.RegisterModule<DependenciesModules>();

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

            // Make sure we've got the Fresnel dependencies loaded:
            builder.RegisterModule<DependenciesModules>();

            // Now bolt in the consumer's dependencies:
            foreach (var module in dependencyModules)
            {
                builder.RegisterModule(module);
            }

            return builder.Build();
        }

    }
}
