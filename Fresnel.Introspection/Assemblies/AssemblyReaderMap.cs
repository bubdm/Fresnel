using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    /// <summary>
    /// A collection of AssemblyReader objects. Each entry is keyed by an Assembly object
    /// </summary>

    public class AssemblyReaderMap : Dictionary<Assembly, AssemblyReader>
    {
        private AssemblyReaderBuilder _AssemblyReaderBuilder;

        public AssemblyReaderMap
        (
            AssemblyReaderBuilder assemblyReaderBuilder
        )
        {
            _AssemblyReaderBuilder = assemblyReaderBuilder;
        }

        public new AssemblyReader this[Assembly assembly]
        {
            get
            {
                var reader = this.TryGetValueOrNull(assembly);
                if (reader == null)
                {
                    reader = _AssemblyReaderBuilder.BuildFor(assembly, false);
                    this.Add(assembly, reader);
                }

                return reader;
            }
        }

        public AssemblyReader this[Type key]
        {
            get { return this[key.Assembly]; }
        }

        public AssemblyReader this[IClassTemplate template]
        {
            get { return this[template.RealObjectType.Assembly]; }
        }

        //public AssemblyReader this[string assemblyName]
        //{
        //    get
        //    {
        //        var match = this.Values.SingleOrDefault(ar => ar.Assembly.GetName() == assemblyName);
        //        return match;
        //    }
        //}

        //public bool Contains(AssemblyName assemblyName)
        //{
        //    var match = this.Values.SingleOrDefault(ar => ar.Assembly.GetName() == assemblyName);
        //    return match != null;
        //}

        /// <summary>
        /// Returns all AssemblyReaders that contain Domain Assemblies
        /// </summary>

        public IEnumerable<AssemblyReader> GetDomainAssemblies()
        {
            // Sort so that the Assemblies with more Domain Classes are higher in the list:
            var matches = this.Values.Where(p => p.IsFrameworkAssembly == false &&
                                                      p.ContainsDomainClasses)
                                             .OrderByDescending(p => p.TemplateCount);
            return matches;
        }

        public void Dispose()
        {
            foreach (var reader in this.Values)
            {
                reader.Dispose();
            }
        }

    }

}
