using Envivo.Fresnel.Introspection.Assemblies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetClassHierarchyCommand
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private NamespaceHierarchyBuilder _NamespaceHierarchyBuilder;

        public GetClassHierarchyCommand
            (
            AssemblyReaderMap assemblyReaderMap,
            NamespaceHierarchyBuilder namespaceHierarchyBuilder
            )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _NamespaceHierarchyBuilder = namespaceHierarchyBuilder;
        }

        public HierarchyNode Invoke()
        {
            var assemblyReader = _AssemblyReaderMap.Values.First(a => !a.IsFrameworkAssembly);
            var result = _NamespaceHierarchyBuilder.BuildFor(assemblyReader.Assembly);
            return result;
        }

        public HierarchyNode Invoke(Assembly domainAssembly)
        {
            var result = _NamespaceHierarchyBuilder.BuildFor(domainAssembly);
            return result;
        }

    }
}
