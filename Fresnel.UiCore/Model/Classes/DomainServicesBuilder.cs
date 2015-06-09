using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class DomainServicesBuilder
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private DomainServiceItemBuilder _DomainServiceItemBuilder;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;

        public DomainServicesBuilder
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            DomainServiceItemBuilder domainServiceItemBuilder,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder
        )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _DomainServiceItemBuilder = domainServiceItemBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
        }

        public GetDomainServicesResponse BuildFor(AssemblyReader assemblyReader)
        {
            try
            {
                var namespaces = new List<Namespace>();

                var domainServiceTypes = assemblyReader.Assembly.GetTypes()
                                        .Where(t => t.IsDerivedFrom<IDomainService>())
                                        .ToArray();

                var tDomainServices = new List<IClassTemplate>();
                foreach (var domainServiceType in domainServiceTypes)
                {
                    var tService = _TemplateCache.GetTemplate(domainServiceType);
                    tDomainServices.Add(tService);
                }

                var nodesGroupedByNamespace = tDomainServices
                                                .GroupBy(t => t.RealType.Namespace);

                foreach (var group in nodesGroupedByNamespace)
                {
                    var classItems = new List<ClassItem>();
                    foreach (var node in group)
                    {
                        var tClass = (ClassTemplate)_TemplateCache.GetTemplate(node.RealType);
                        var item = _DomainServiceItemBuilder.BuildFor(tClass);
                        classItems.Add(item);
                    }

                    var namespaceFriendlyName = group.Key.Split('.').Last();

                    var newNamespace = new Namespace()
                    {
                        FullName = group.Key,
                        Name = namespaceFriendlyName,
                        Classes = classItems.ToArray()
                    };
                    namespaces.Add(newNamespace);
                }

                var oDomainServices = _ObserverCache.GetAllObservers()
                                        .OfType<ObjectObserver>()
                                        .Where(o => o.Template.RealType.IsDerivedFrom<IDomainService>());

                var modifications = _ModificationsBuilder.BuildFrom(oDomainServices, 0);

                return new GetDomainServicesResponse
                {
                    Modifications = modifications,
                    Passed = true,
                    Namespaces = namespaces.OrderBy(n => n.Name)
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GetDomainServicesResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}