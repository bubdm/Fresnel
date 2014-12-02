using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.ClassHierarchy;
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
        private ClassHierarchyItemBuilder _ClassHierarchyItemBuilder;

        public GetClassHierarchyCommand
            (
            AssemblyReaderMap assemblyReaderMap,
            NamespaceHierarchyBuilder namespaceHierarchyBuilder,
            ClassHierarchyItemBuilder classHierarchyItemBuilder
            )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _NamespaceHierarchyBuilder = namespaceHierarchyBuilder;
            _ClassHierarchyItemBuilder = classHierarchyItemBuilder;
        }

        public IEnumerable<ClassHierarchyItem> Invoke()
        {
            var assemblyReader = _AssemblyReaderMap.Values.First(a => !a.IsFrameworkAssembly);
            var hierarchyNodes = _NamespaceHierarchyBuilder.BuildListFor(assemblyReader.Assembly);

            // Exclude any namespace nodes that don't have actual classes:
            var itemsToExclude = hierarchyNodes.Where(n => n.IsNamespace &&
                                                            n.Children.Count() == 1 &&
                                                            n.Children.First().IsNamespace);

            hierarchyNodes = hierarchyNodes.Except(itemsToExclude);

            var results = hierarchyNodes.Select(h => _ClassHierarchyItemBuilder.BuildFor(h));
            return results;
        }

        public IEnumerable<HierarchyNode> Invoke(Assembly domainAssembly)
        {
            var results = _NamespaceHierarchyBuilder.BuildListFor(domainAssembly);
            return results;
        }

    }
}
