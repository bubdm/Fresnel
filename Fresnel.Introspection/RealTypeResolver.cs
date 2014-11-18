using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection
{

    /// <summary>
    /// Returns the true Type of a proxied type
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class RealTypeResolver
    {
        private List<ITypeResolverStrategy> _Strategies = new List<ITypeResolverStrategy>();
        
        public void Register(ITypeResolverStrategy strategy)
        {
            _Strategies.Add(strategy);
        }

        public Type GetRealType(Type proxyType)
        {
            // TODO: Loop through the resolvers until we find one that works
            return proxyType;
        }
    }

}
