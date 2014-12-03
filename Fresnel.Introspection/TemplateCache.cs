using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection
{

    /// <summary>
    /// Creates and returns Class Templates for all known Domain Assemblies
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class TemplateCache
    {
        private RealTypeResolver _RealTypeResolver;
        private AssemblyReaderMap _AssemblyReaders = null;

        public TemplateCache
            (
            RealTypeResolver realTypeResolver,
            AssemblyReaderMap assemblyReaders
            )
        {
            if (realTypeResolver == null)
                throw new ArgumentNullException("realTypeResolver");

            if (assemblyReaders == null)
                throw new ArgumentNullException("assemblyReaders");

            _RealTypeResolver = realTypeResolver;
            _AssemblyReaders = assemblyReaders;
        }

        /// <summary>
        /// Returns a Template from the cache for the given object Type. A new Template is created if one does not already exist in the cache.
        /// </summary>
        /// <param name="realObjectType">The Type of the Object</param>
        /// <returns>A Template for the given object Type</returns>
        /// <remarks>
        /// </remarks>
        public IClassTemplate GetTemplate(Type objectType)
        {
            // We may have been given a proxy type, so make sure we know the real type:
            var realType = _RealTypeResolver.GetRealType(objectType);

            var assemblyReader = _AssemblyReaders[realType.Assembly];
            return assemblyReader.GetTemplate(realType);
        }

        public IClassTemplate GetTemplate<T>()
            where T : class
        {
            var objectType = typeof(T);
            return this.GetTemplate(objectType);
        }

        public IClassTemplate GetTemplate(string fullyQualifiedName)
        {
            Type type = null;
            foreach (var reader in _AssemblyReaders.Values)
            {
                type = reader.Assembly.GetType(fullyQualifiedName, false, true);
                if (type != null)
                    break;
            }

            return type != null ?
                    this.GetTemplate(type) :
                    null;
        }

        /// <summary>
        /// Returns a list of all ClassTemplates for the given Assembly
        /// </summary>
        /// <param name="domainAssembly">The name of the assembly to read</param>
        /// <returns>A generic sorted list of matching Class Templates</returns>

        public IEnumerable<ClassTemplate> GetAllTemplates(Assembly domainAssembly)
        {
            // Delegate the request to the appropriate AssemblyReader:
            return _AssemblyReaders[domainAssembly].GetTemplates();
        }

        /// <summary>
        /// Returns the assembly name for the given class name
        /// </summary>
        /// <param name="className"></param>


        private string ExtractAssemblyName(string className)
        {
            var lastPeriodPos = className.LastIndexOf('.');
            if (lastPeriodPos > 0)
            {
                return className.Substring(0, lastPeriodPos);
            }
            else
            {
                return className;
            }
        }

    }

}
