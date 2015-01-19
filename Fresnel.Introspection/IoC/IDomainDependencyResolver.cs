using System;

namespace Envivo.Fresnel.Introspection.IoC
{
    public interface IDomainDependencyResolver
    {
        object Resolve(Type classType);
    }
}