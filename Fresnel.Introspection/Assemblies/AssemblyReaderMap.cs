using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    /// <summary>
    /// A collection of AssemblyReader objects. Each entry is keyed by an Assembly object
    /// </summary>
    
    public class AssemblyReaderMap : Dictionary<Assembly, AssemblyReader>
    {
        private readonly List<AssemblyReader> _AssemblyReaders;

        public AssemblyReaderMap()
        {
            _AssemblyReaders = new List<AssemblyReader>();
        }

        public AssemblyReader this[Type key]
        {
            get { return this[key.Assembly]; }
        }

        public AssemblyReader this[IClassTemplate template]
        {
            get { return this[template.RealObjectType.Assembly]; }
        }

        public AssemblyReader this[string assemblyName]
        {
            get
            {
                foreach (var reader in _AssemblyReaders)
                {
                    if (reader.Assembly.FullName == assemblyName)
                    {
                        return reader;
                    }
                }
                return null;
            }
        }

        public AssemblyReader this[int index]
        {
            get { return _AssemblyReaders[index]; }
        }

        public bool Contains(AssemblyName assemblyName)
        {
            foreach (var reader in _AssemblyReaders)
            {
                if (reader.Assembly.GetName() == assemblyName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns all AssemblyReaders that contain Domain Assemblies
        /// </summary>
        
        public IEnumerable<AssemblyReader> GetDomainAssemblies()
        {
            // Sort so that the Assemblies with more Domain Classes are higher in the list:
            var matches = _AssemblyReaders.Where(p => p.IsFrameworkAssembly == false &&
                                                      p.ContainsDomainClasses)
                                             .OrderByDescending(p => p.TemplateCount);
            return matches;
        }

        public void Dispose()
        {
            foreach (var reader in _AssemblyReaders)
            {
                reader.Dispose();
            }
        }

    }

}
