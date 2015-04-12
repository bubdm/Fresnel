using Autofac;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using System;
using System.Linq;

namespace Envivo.Fresnel.CompositionRoot.DomainIoC
{
    public class DomainObjectFactory : IDomainObjectFactory
    {
        private IComponentContext _ComponentContext;
        private RealTypeResolver _RealTypeResolver;

        public DomainObjectFactory
            (
            IComponentContext componentContext,
            RealTypeResolver realTypeResolver
            )
        {
            _ComponentContext = componentContext;
            _RealTypeResolver = realTypeResolver;
        }

        public object Create(Type classType, params object[] args)
        {
            if (!_ComponentContext.IsRegistered(classType))
                return null;

            object result = null;
            if (args == null || !args.Any())
            {
                result = _ComponentContext.Resolve(classType);
            }
            else
            {
                // Note that we're using the RealTypeResolver, incase the object is a dynamic proxy:
                var ctorParams = args.Select(a => new TypedParameter(_RealTypeResolver.GetRealType(a), a));
                result = _ComponentContext.Resolve(classType, ctorParams);
            }

            return result;
        }
    }
}