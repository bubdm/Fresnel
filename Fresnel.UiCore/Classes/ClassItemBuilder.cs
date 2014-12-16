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
            create.IsEnabled = tClass.HasDefaultConstructor;
            create.Tooltip = create.IsEnabled ? "Create a new " + tClass.FriendlyName : "This item is not creatable. Consider using a sub item instead.";
            create.CommandUri = create.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = create.IsEnabled ? tClass.FullName : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = true;
            search.Tooltip = search.IsEnabled ? "Search for existing instances of " + tClass.FriendlyName : "This item cannot be searched for";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)

            return item;
        }

    }
}
