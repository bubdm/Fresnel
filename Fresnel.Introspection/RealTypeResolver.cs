using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection
{

    /// <summary>
    /// Returns the true Type of a proxied object
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class RealTypeResolver
    {
        private List<IRealTypeResolver> _Resolvers = new List<IRealTypeResolver>();

        /// <summary>
        /// Adds the given Type Resolver strategy to the internal list of strategies
        /// </summary>
        /// <param name="resolver"></param>
        public void Register(IRealTypeResolver resolver)
        {
            _Resolvers.Add(resolver);
        }

        public Type GetRealType(Type proxyType)
        {
            if (proxyType == null)
                return null;

            foreach (var resolver in _Resolvers)
            {
                var realType = resolver.GetRealType(proxyType);
                if (realType != null && realType != proxyType)
                {
                    return realType;
                }
            }

            // Couldn't find one, so return the original Type:
            return proxyType;
        }
    }

}
