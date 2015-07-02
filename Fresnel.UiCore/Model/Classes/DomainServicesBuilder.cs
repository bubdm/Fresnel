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
        private ObserverRetriever _ObserverRetriever;
        private DomainServiceItemBuilder _DomainServiceItemBuilder;

        public DomainServicesBuilder
            (
            TemplateCache templateCache,
            DomainServiceItemBuilder domainServiceItemBuilder
        )
        {
            _TemplateCache = templateCache;
            _DomainServiceItemBuilder = domainServiceItemBuilder;
        }

        public IEnumerable<Namespace> BuildFor(AssemblyReader assemblyReader)
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

            var results = namespaces.OrderBy(n => n.Name);
            return results;
        }
    }
}