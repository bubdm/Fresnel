﻿using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class ClassItemBuilder
    {
        private TemplateCache _TemplateCache;
        private IDomainDependencyResolver _DomainDependencyResolver;
        private MethodVmBuilder _MethodVmBuilder;
        private CanCreatePermission _CanCreatePermission;

        public ClassItemBuilder
            (
            TemplateCache templateCache,
            IDomainDependencyResolver domainDependencyResolver,
            MethodVmBuilder methodVmBuilder,
            CanCreatePermission canCreatePermission
            )
        {
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

            var createCheck = _CanCreatePermission.IsSatisfiedBy(tClass);

            var create = item.Create = new InteractionPoint();
            create.IsVisible = true;
            create.IsEnabled = createCheck == null;
            create.Tooltip = create.IsEnabled ? "Create a new instance of " + tClass.FriendlyName : createCheck.Message;
            create.CommandUri = create.IsEnabled ? "/Toolbox/Create" : "";
            create.CommandArg = create.IsEnabled ? tClass.FullName : "";

            var search = item.Search = new InteractionPoint();
            search.IsVisible = true;
            search.IsEnabled = tClass.IsPersistable;
            search.Tooltip = search.IsEnabled ? "Search for existing instances of " + tClass.FriendlyName : "These items are not saved to the database";

            // TODO: Add other Interaction Points (Factory, Service, Static methods, etc)
            item.FactoryCommands = this.CreateFactoryCommands(tClass);

            return item;
        }

        public DependencyMethodVM[] CreateFactoryCommands(ClassTemplate tClass)
        {
            var genericFactory = typeof(IFactory<>);
            var factoryType = genericFactory.MakeGenericType(tClass.RealType);
            var factory = _DomainDependencyResolver.Resolve(factoryType);

            if (factory == null)
                return null;

            var tFactory = (ClassTemplate)_TemplateCache.GetTemplate(factory.GetType());
            var results = new List<DependencyMethodVM>();

            var visibleMethods = tFactory.Methods.VisibleOnly;
            foreach (var tMethod in visibleMethods)
            {
                var methodVM = _MethodVmBuilder.BuildFor(tFactory, tMethod);
                results.Add(methodVM);
            }
            return results.ToArray();
        }


    }
}