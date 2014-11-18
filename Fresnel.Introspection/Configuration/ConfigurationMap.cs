using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Assemblies;

namespace Envivo.Fresnel.Configuration
{

    /// <summary>
    /// Contains all configurations associated with an Assembly
    /// </summary>
    public class ConfigurationMap
    {
        private readonly Dictionary<Type, IClassConfiguration> _ClassConfigurationMap = new Dictionary<Type, IClassConfiguration>();

        /// <summary>
        /// The Assembly these configurations belong to
        /// </summary>
        public Assembly Assembly { get; private set; }

        public IApplicationConfiguration tApplicationConfiguration { get; internal set; }

        /// <summary>
        /// Returns the IAssemblyConfiguration for the given Assembly
        /// </summary>
        
        public IAssemblyConfiguration GetAssemblyConfiguration { get; internal set; }

        /// <summary>
        /// Returns the IAssemblyConfiguration for the given Class
        /// </summary>
        /// <param name="classType"></param>
        
        public IClassConfiguration GetClassConfiguration(Type classType)
        {
            return _ClassConfigurationMap.TryGetValueOrNull(classType);
        }

        internal void SetClassConfiguration(Type classType, IClassConfiguration configuration)
        {
            _ClassConfigurationMap[classType] = configuration;
        }

    }

}
