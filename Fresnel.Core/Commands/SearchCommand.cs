using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Commands
{
    public class SearchCommand
    {
        private IPersistenceService _PersistenceService;

        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private IDomainDependencyResolver _DomainDependencyResolver;

        public SearchCommand
        (
            IPersistenceService persistenceService,

            TemplateCache templateCache,
            ObserverCache observerCache,
            IDomainDependencyResolver domainDependencyResolver
        )
        {
            _PersistenceService = persistenceService;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _DomainDependencyResolver = domainDependencyResolver;
        }

        public IQueryable Search(ClassTemplate tClass)
        {
            this.CheckIfTypeIsRecognised(tClass.RealType);

            return _PersistenceService.GetObjects(tClass.RealType);
        }

        public IQueryable Search(BasePropertyObserver oProp)
        {
            var querySpecification = this.GetQuerySpecification(oProp.Template.Attributes);
            var oParent = oProp.OuterObject;

            var searchType = oProp.Template.IsCollection ?
                                ((CollectionTemplate)oProp.Template.InnerClass).ElementType :
                                oProp.Template.PropertyType;

            this.CheckIfTypeIsRecognised(searchType);

            var results = this.GetResults(querySpecification, oParent) ??
                          _PersistenceService.GetObjects(searchType);
            return results;
        }

        public IQueryable Search(ParameterObserver oParam)
        {
            var querySpecification = this.GetQuerySpecification(oParam.Template.Attributes);
            var oParent = oParam.OuterMethod.OuterObject;

            var searchType = oParam.Template.IsCollection ?
                                ((CollectionTemplate)oParam.Template.InnerClass).ElementType :
                                oParam.Template.ParameterType;

            this.CheckIfTypeIsRecognised(searchType);

            var results = this.GetResults(querySpecification, oParent) ??
                          _PersistenceService.GetObjects(searchType);
            return results;
        }

        public IQueryable Search(Type querySpecificationType, ObjectObserver oRequestor)
        {
            var querySpecification = _DomainDependencyResolver.Resolve(querySpecificationType);
            if (querySpecification == null)
                return null;

            var results = this.GetResults(querySpecification, oRequestor);
            return results;
        }

        private object GetQuerySpecification(AttributesMap attributes)
        {
            var querySpecType = attributes.Get<FilterQuerySpecificationAttribute>().SpecificationType;
            if (querySpecType == null)
                return null;

            var querySpec = _DomainDependencyResolver.Resolve(querySpecType);
            return querySpec;
        }

        private void CheckIfTypeIsRecognised(Type classType)
        {
            if (!_PersistenceService.IsTypeRecognised(classType))
                throw new CoreException(string.Concat(_PersistenceService.GetType().Name, " does not recognise ", classType.FullName));
        }

        private IQueryable GetResults(object querySpecification, ObjectObserver oParent)
        {
            if (querySpecification == null)
                return null;

            var tQuerySpec = (ClassTemplate)_TemplateCache.GetTemplate(querySpecification.GetType().FullName);

            IQueryable results = null;
            if (oParent != null)
            {
                // First look for a method that accepts the given Object as an argument:
                var tParams = new ClassTemplate[] { oParent.Template };
                var tMethod = tQuerySpec.Methods.FindMethodThatAccepts(tParams);
                if (tMethod != null)
                {
                    var args = new object[] { oParent.RealObject };
                    results = (IQueryable)tMethod.Invoke(querySpecification, args);
                }
            }

            if (results == null)
            {
                // Look for a method that accepts no args:
                var tMethod = tQuerySpec.Methods.Values.Single(m => !m.Parameters.Any());
                if (tMethod != null)
                {
                    results = (IQueryable)tMethod.Invoke(querySpecification, null);
                }
            }

            return results;
        }

    }
}