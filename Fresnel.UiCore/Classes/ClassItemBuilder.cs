using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Classes
{
    public class ClassItemBuilder
    {
        private TemplateCache _TemplateCache;

        public ClassItemBuilder
            (
            TemplateCache templateCache
            )
        {
            _TemplateCache = templateCache;
        }

        public ClassItem BuildFor(ClassTemplate tClass)
        {
            var item = new ClassItem()
            {
                Name = tClass.FriendlyName,
                TypeName = tClass.FullName,
                Description = tClass.XmlComments.Summary,
                Tooltip = tClass.XmlComments.Remarks,
                IsEnabled = true,
                IsVisible = tClass.IsVisible,
            };

            var create = item.Create = new InteractionPoint();
            create.IsVisible = true;
            create.IsEnabled = tClass.IsCreatable;
            create.Tooltip = item.IsEnabled ? "Create a new " + tClass.FriendlyName : "This item is not creatable. Consider using a sub item instead.";
            create.CommandUri = item.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = item.IsEnabled ? tClass.FullName : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = true;
            search.Tooltip = item.IsEnabled ? "Search for existing instances of " + tClass.FriendlyName : "This item cannot be searched for";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)

            return item;
        }

    }
}
