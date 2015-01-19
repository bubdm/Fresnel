using Autofac;
using Envivo.Fresnel.Introspection.IoC;
using System;
using System.Linq;

namespace Envivo.Fresnel.Bootstrap.DomainIoC
{
    public class DomainObjectFactory : IDomainObjectFactory
    {
        private IComponentContext _ComponentContext;

        public DomainObjectFactory
            (
            IComponentContext componentContext
            )
        {
            _ComponentContext = componentContext;
        }

        public object Create(Type classType, params object[] args)
        {
            if (!_ComponentContext.IsRegistered(classType))
                return null;

            var ctorParams = args.Select(a => new TypedParameter(a.GetType(), a));

            var result = _ComponentContext.Resolve(classType, ctorParams);
            return result;
        }
    }
}