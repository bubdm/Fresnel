using Autofac;

using Envivo.Fresnel.Introspection.IoC;

using System.Collections.Generic;

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
            return this.Build(new Module[] { });
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

            // THIS SEEMS TO BE VERY SLOW, HENCE IT IS DISABLED:
            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // Now bolt in the Consumer's dependencies:
            foreach (var module in dependencyModules)
            {
                builder.RegisterModule(module);
            }

            var result = builder.Build();

            // This allows us to access the Container elsewhere in the code (necessary for adding late registrations):
            this.RegisterContainer(result);

            this.SetupContainer(result);

            return result;
        }

        private void RegisterContainer(IContainer existingContainer)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(existingContainer).SingleInstance();
            builder.Update(existingContainer);
        }

        private void SetupContainer(IContainer container)
        {
        }

        private void RegisterMandatoryModules(ContainerBuilder builder)
        {
            builder.RegisterModule<IntrospectionDependencies>();
            builder.RegisterModule<CoreDependencies>();
            builder.RegisterModule<UiCoreDependencies>();

            builder.RegisterType<DomainIoC.DomainClassRegistrar>().As<IDomainClassRegistrar>().SingleInstance();
            builder.RegisterType<DomainIoC.DomainObjectFactory>().As<IDomainObjectFactory>().SingleInstance();
            builder.RegisterType<DomainIoC.DomainDependencyRegistrar>().As<IDomainDependencyRegistrar>().SingleInstance();
            builder.RegisterType<DomainIoC.DomainDependencyResolver>().As<IDomainDependencyResolver>().SingleInstance();
        }
    }
}