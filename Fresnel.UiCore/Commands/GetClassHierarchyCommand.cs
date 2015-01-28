using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Model.Classes;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetClassHierarchyCommand
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private NamespacesBuilder _NamespacesBuilder;

        public GetClassHierarchyCommand
            (
            AssemblyReaderMap assemblyReaderMap,
            NamespacesBuilder namespacesBuilder
            )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _NamespacesBuilder = namespacesBuilder;
        }

        public IEnumerable<Namespace> Invoke()
        {
            var assemblyReader = _AssemblyReaderMap.Values.First(a => !a.IsFrameworkAssembly);
            var results = _NamespacesBuilder.BuildFor(assemblyReader);
            return results;
        }

        public IEnumerable<Namespace> Invoke(Assembly domainAssembly)
        {
            var assemblyReader = _AssemblyReaderMap.TryGetValueOrNull(domainAssembly);
            if (assemblyReader == null)
                return null;

            var results = _NamespacesBuilder.BuildFor(assemblyReader);
            return results;
        }
    }
}