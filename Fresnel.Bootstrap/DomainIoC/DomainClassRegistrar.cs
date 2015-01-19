using Autofac;
using Envivo.Fresnel.Introspection.IoC;
using System;

namespace Envivo.Fresnel.Bootstrap.DomainIoC
{
    public class DomainClassRegistrar : IDomainClassRegistrar
    {
        private IContainer _ExistingContainer;

        public DomainClassRegistrar
            (
            IContainer existingContainer
            )
        {
            _ExistingContainer = existingContainer;
        }

        public void RegisterTypes(Type[] domainClassTypes)
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterTypes(domainClassTypes)
                .InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.Update(_ExistingContainer);
        }
    }
}