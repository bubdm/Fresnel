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
        private DomainServiceObserverRetriever _DomainServiceObserverRetriever;
        private ObjectVmBuilder _ObjectVmBuilder;
        private MethodVmBuilder _MethodVmBuilder;
        private CanCreatePermission _CanCreatePermission;

        public DomainServiceItemBuilder
            (
            DomainServiceObserverRetriever domainServiceObserverRetriever,
            ObjectVmBuilder objectVmBuilder,
            MethodVmBuilder methodVmBuilder,
            CanCreatePermission canCreatePermission
            )
        {
            _DomainServiceObserverRetriever = domainServiceObserverRetriever;
            _ObjectVmBuilder = objectVmBuilder;
            _MethodVmBuilder = methodVmBuilder;
            _CanCreatePermission = canCreatePermission;
        }

        public ServiceClassItem BuildFor(ClassTemplate tServiceClass)
        {
            var oDomainService = (ObjectObserver)_DomainServiceObserverRetriever.GetObserver(tServiceClass.RealType);

            var item = new ServiceClassItem()
            {
                Name = tServiceClass.FriendlyName,
                Type = tServiceClass.RealType.Name,
                FullTypeName = tServiceClass.FullName,
                Description = tServiceClass.XmlComments.Summary,
                Tooltip = tServiceClass.XmlComments.Remarks,
                IsEnabled = true,
                IsVisible = tServiceClass.IsVisible,
                AssociatedService =_ObjectVmBuilder.BuildFor(oDomainService)
            };

            return item;
        }
    }
}