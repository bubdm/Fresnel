using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// Returns Observers for .NET Domain Services
    /// </summary>
    public class DomainServiceObserverRetriever
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private Lazy<IEnumerable<IDomainService>> _DomainServices;

        public DomainServiceObserverRetriever
        (
            TemplateCache templateCache,
            ObserverCache observerCache,
            Lazy<IEnumerable<IDomainService>> domainServices
        )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _DomainServices = domainServices;
        }

        /// <summary>
        /// Returns an Observer for a Domain Service.
        /// </summary>
        /// <param name="domainServiceType"></param>
        /// <returns></returns>
        public ObjectObserver GetObserver(Type domainServiceType)
        {
            var oObject = _ObserverCache.GetServiceObserver(domainServiceType);

            var domainService = _DomainServices.Value.Single(ds => ds.GetType() == domainServiceType);
            oObject.SetRealObject(domainService);

            return oObject;
        }

        /// <summary>
        /// Returns an Observer for a Domain Service.
        /// </summary>
        /// <param name="domainServiceFullTypeName"></param>
        /// <returns></returns>
        public ObjectObserver GetObserver(string domainServiceFullTypeName)
        {
            var tServiceClass = _TemplateCache.GetTemplate(domainServiceFullTypeName);

            var oObject = this.GetObserver(tServiceClass.RealType);
            return oObject;
        }

    }
}