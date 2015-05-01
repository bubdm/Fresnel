using Autofac;
using Envivo.Fresnel.Introspection.IoC;
using System;

namespace Envivo.Fresnel.CompositionRoot.DomainIoC
{
    public class DomainDependencyRegistrar : IDomainDependencyRegistrar
    {
        private IContainer _ExistingContainer;

        public DomainDependencyRegistrar
            (
            IContainer existingContainer
            )
        {
            _ExistingContainer = existingContainer;
        }

        public void RegisterTypes(Type[] domainDependencyTypes)
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterTypes(domainDependencyTypes)
                .AsImplementedInterfaces()
                .AsSelf()
                //.InstancePerRequest() // This will cause failures, as SingleInstance cannot depend on InstancePerRequest
                .InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.Update(_ExistingContainer);
        }
    }
}