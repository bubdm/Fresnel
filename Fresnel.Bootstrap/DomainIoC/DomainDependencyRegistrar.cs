using Autofac;
using Autofac.Core.Registration;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap.DomainIoC
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
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.Update(_ExistingContainer);
        }

    }
}
