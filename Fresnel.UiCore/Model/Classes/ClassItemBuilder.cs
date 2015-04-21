using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class ClassItemBuilder
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private IDomainDependencyResolver _DomainDependencyResolver;
        private MethodVmBuilder _MethodVmBuilder;
        private CanCreatePermission _CanCreatePermission;

        public ClassItemBuilder
            (
            ObserverCache observerCache,
            TemplateCache templateCache,
            IDomainDependencyResolver domainDependencyResolver,
            MethodVmBuilder methodVmBuilder,
            CanCreatePermission canCreatePermission
            )
        {
            _ObserverCache = observerCache;
            _TemplateCache = templateCache;
            _DomainDependencyResolver = domainDependencyResolver;
            _MethodVmBuilder = methodVmBuilder;
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

            // NB: Don't show the default "Create" option if there are Factory methods:
            item.FactoryMethods = this.CreateFactoryMethods(tClass);
            var areFactoryMethodsAvailable = item.FactoryMethods != null;

            var createPermissionError = _CanCreatePermission.IsSatisfiedBy(tClass);
            var create = item.Create = new InteractionPoint();
            create.IsVisible = !areFactoryMethodsAvailable;
            create.IsEnabled = createPermissionError == null;
            create.Tooltip = create.IsEnabled ? "Create a new instance of " + tClass.FriendlyName : createPermissionError.Message;
            create.CommandUri = create.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = create.IsEnabled ? tClass.FullName : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = tClass.IsPersistable;
            search.Tooltip = search.IsEnabled ? "Search for existing instances of " + tClass.FriendlyName : "These items are not saved to the database";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)

            return item;
        }

        public MethodVM[] CreateFactoryMethods(ClassTemplate tClass)
        {
            var genericFactory = typeof(IFactory<>);
            var factoryType = genericFactory.MakeGenericType(tClass.RealType);
            var factory = _DomainDependencyResolver.Resolve(factoryType);

            if (factory == null)
                return null;

            var tFactory = (ClassTemplate)_TemplateCache.GetTemplate(factory.GetType());
            var results = new List<MethodVM>();

            var oFactory = (ObjectObserver)_ObserverCache.GetObserver(factory, tFactory.RealType);
            oFactory.IsPinned = true;

            foreach (var oMethod in oFactory.Methods.Values)
            {
                if (!oMethod.Template.IsVisible)
                    continue;

                var methodVM = _MethodVmBuilder.BuildFor(oMethod);
                results.Add(methodVM);
            }
            return results.ToArray();
        }


    }
}