using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.ClassHierarchy
{
    public class ClassHierarchyItemBuilder
    {
        private TemplateCache _TemplateCache;

        public ClassHierarchyItemBuilder
            (
            TemplateCache templateCache
            )
        {
            _TemplateCache = templateCache;
        }

        public ClassHierarchyItem BuildFor(HierarchyNode hierarchyNode)
        {
            var tClass = hierarchyNode.IsClass ?
                            _TemplateCache.GetTemplate(hierarchyNode.Type)
                            : null;

            var item = new ClassHierarchyItem()
            {
                Name = hierarchyNode.Name,
                TypeName = tClass != null ? hierarchyNode.Type.FullName : "",
                Description = tClass != null ? tClass.Summary : "",
                Tooltip = tClass != null ? tClass.Remarks : "",
                IsEnabled = hierarchyNode.IsClass,
                IsVisible = hierarchyNode.Children.Any(),
            };

            var create = item.Create = new InteractionPoint();
            create.IsVisible = true;
            create.IsEnabled = hierarchyNode.IsClass && !hierarchyNode.IsAbstract;
            create.Tooltip = item.IsEnabled ? "Create a new " + hierarchyNode.Name : "This item is not creatable. Consider using a sub item instead.";
            create.CommandUri = item.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = item.IsEnabled ? hierarchyNode.FQN : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = hierarchyNode.IsClass;
            search.Tooltip = item.IsEnabled ? "Search for existing instances of " + hierarchyNode.Name : "This item cannot be searched for";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)

            return item;
        }

    }
}
