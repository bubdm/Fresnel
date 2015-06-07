using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.Classes
{
    public class DomainServiceItemBuilder
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private IEnumerable<IDomainService> _DomainServiceInstances;
        private MethodVmBuilder _MethodVmBuilder;
        private CanCreatePermission _CanCreatePermission;

        public DomainServiceItemBuilder
            (
            ObserverCache observerCache,
            TemplateCache templateCache,
            IEnumerable<IDomainService> domainServiceInstances,
            MethodVmBuilder methodVmBuilder,
            CanCreatePermission canCreatePermission
            )
        {
            _ObserverCache = observerCache;
            _TemplateCache = templateCache;
            _DomainServiceInstances = domainServiceInstances;
            _MethodVmBuilder = methodVmBuilder;
            _CanCreatePermission = canCreatePermission;
        }

        public ClassItem BuildFor(ClassTemplate tServiceClass)
        {
            var item = new ClassItem()
            {
                Name = tServiceClass.FriendlyName,
                Type = tServiceClass.RealType.Name,
                FullTypeName = tServiceClass.FullName,
                Description = tServiceClass.XmlComments.Summary,
                Tooltip = tServiceClass.XmlComments.Remarks,
                IsEnabled = true,
                IsVisible = tServiceClass.IsVisible,
            };

            item.ServiceMethods = this.CreateServiceMethods(tServiceClass);
            var areFactoryMethodsAvailable = item.FactoryMethods != null;

            return item;
        }

        public MethodVM[] CreateServiceMethods(ClassTemplate tServiceClass)
        {
            var domainService = _DomainServiceInstances
                            .SingleOrDefault(ds => ds.GetType().IsDerivedFrom(tServiceClass.RealType));

            if (domainService == null)
                return null;

            var tDomainService = (ClassTemplate)_TemplateCache.GetTemplate(domainService.GetType());
            var results = new List<MethodVM>();

            var oDomainService = (ObjectObserver)_ObserverCache.GetObserver(domainService, tDomainService.RealType);
            oDomainService.IsPinned = true;

            foreach (var oMethod in oDomainService.Methods.Values)
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