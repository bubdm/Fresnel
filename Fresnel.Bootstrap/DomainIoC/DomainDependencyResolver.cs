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

        public object Resolve(Type classType)
        {
            var result = _ComponentContext.Resolve(classType);
            return result;
        }
    }
}