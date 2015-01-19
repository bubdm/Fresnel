using System;

namespace Envivo.Fresnel.Introspection.IoC
{
    public interface IDomainObjectFactory
    {
        object Create(Type classType, params object[] args);
    }
}