using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class NamespacesBuilder
    {
        private TemplateCache _TemplateCache;
        private NamespaceHierarchyBuilder _NamespaceHierarchyBuilder;
        private ClassItemBuilder _ClassItemBuilder;

        public NamespacesBuilder
            (
            TemplateCache templateCache,
            NamespaceHierarchyBuilder namespaceHierarchyBuilder,
            ClassItemBuilder classItemBuilder
            )
        {
            _TemplateCache = templateCache;
            _NamespaceHierarchyBuilder = namespaceHierarchyBuilder;
            _ClassItemBuilder = classItemBuilder;
        }

        public IEnumerable<Namespace> BuildFor(AssemblyReader assemblyReader)
        {
            var results = new List<Namespace>();

            var hierarchy = _NamespaceHierarchyBuilder.BuildListFor(assemblyReader.Assembly);

            var nodesGroupedByNamespace = hierarchy
                                            .Where(h => h.Type != null)
                                            .GroupBy(h => h.Type.Namespace);

            var commonNamespacePrexix = this.DetermineCommonNamespacePrefix(hierarchy);

            foreach (var group in nodesGroupedByNamespace)
            {
                var classItems = new List<ClassItem>();
                foreach (var node in group)
                {
                    var tClass = (ClassTemplate)_TemplateCache.GetTemplate(node.Type);
                    var item = _ClassItemBuilder.BuildFor(tClass);
                    classItems.Add(item);
                }

                var namespaceFriendlyName = group.Key
                                                .Replace(commonNamespacePrexix + ".", null)
                                                .Replace(".", " ");

                var newNamespace = new Namespace()
                {
                    FullName = group.Key,
                    Name = namespaceFriendlyName,
                    Classes = classItems.ToArray()
                };
                results.Add(newNamespace);
            }

            return results
                    .OrderBy(n => n.Name);
        }

        private string DetermineCommonNamespacePrefix(IEnumerable<HierarchyNode> hierarchy)
        {
            var currentNode = hierarchy.First();
            while (currentNode.Children.Count() == 1)
            {
                currentNode = currentNode.Children.First();
            }

            var result = currentNode.FQN;
            return result;
        }
    }
}