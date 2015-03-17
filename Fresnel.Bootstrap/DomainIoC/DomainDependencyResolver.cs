using Autofac;
using Envivo.Fresnel.Introspection.IoC;
using System;

namespace Envivo.Fresnel.Bootstrap.DomainIoC
{
    public class DomainDependencyResolver : IDomainDependencyResolver
    {
        private IComponentContext _ComponentContext;

        public DomainDependencyResolver
            (
            IComponentContext componentContext
            )
        {
            _ComponentContext = componentContext;
        }

        public object Resolve(Type dependencyType)
        {
            if (!_ComponentContext.IsRegistered(dependencyType))
                return null;

            var result = _ComponentContext.Resolve(dependencyType);
            return result;
        }
    }
}