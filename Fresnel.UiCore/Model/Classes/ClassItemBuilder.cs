using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class ClassItemBuilder
    {
        private TemplateCache _TemplateCache;
        private CanCreatePermission _CanCreatePermission;

        public ClassItemBuilder
            (
            TemplateCache templateCache,
            CanCreatePermission canCreatePermission
            )
        {
            _TemplateCache = templateCache;
            _CanCreatePermission = canCreatePermission;
        }

        public ClassItem BuildFor(ClassTemplate tClass)
        {
            var item = new ClassItem()
            {
                Name = tClass.FriendlyName,
                Type = tClass.RealType.Name,
                FullTypeName = tClass.FullName,
                Description = tClass.XmlComments.Summary,
                Tooltip = tClass.XmlComments.Remarks,
                IsEnabled = true,
                IsVisible = tClass.IsVisible,
            };

            var createCheck = _CanCreatePermission.IsSatisfiedBy(tClass);

            var create = item.Create = new InteractionPoint();
            create.IsVisible = true;
            create.IsEnabled = createCheck.Passed;
            create.Tooltip = create.IsEnabled ? "Create a new instance of " + tClass.FriendlyName : createCheck.FailureReason;
            create.CommandUri = create.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = create.IsEnabled ? tClass.FullName : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = tClass.IsPersistable;
            search.Tooltip = search.IsEnabled ? "Search for existing instances of " + tClass.FriendlyName : "This item cannot be searched for";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)

            return item;
        }
    }
}