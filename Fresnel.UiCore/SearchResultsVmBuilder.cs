﻿using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.UiCore
{
    public class SearchResultsVmBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ObserverCache _ObserverCache;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private SearchFilterPropertyVmBuilder _SearchFilterPropertyVmBuilder;
        private PropertyVmBuilder _PropertyVmBuilder;

        public SearchResultsVmBuilder
            (
            RealTypeResolver realTypeResolver,
            ObserverCache observerCache,
            ClassHierarchyBuilder classHierarchyBuilder,
            SearchFilterPropertyVmBuilder searchFilterPropertyVmBuilder,
            PropertyVmBuilder propertyVmBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ObserverCache = observerCache;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _SearchFilterPropertyVmBuilder = searchFilterPropertyVmBuilder;
            _PropertyVmBuilder = propertyVmBuilder;
        }

        public SearchResultsVM BuildFor(IQueryable searchResults, ClassTemplate tElement, SearchRequest originalRequest)
        {
            // Only return back the number of items actually requested:
            var resultsPage = searchResults.ToList<object>().Take(originalRequest.PageSize);

            var oCollection = (CollectionObserver)_ObserverCache.GetObserver(resultsPage, resultsPage.GetType());
            
            // Code smell: This is the only point where we know the persistent state:
            this.MarkAsPersistent(oCollection);

            var allKnownProperties = _ClassHierarchyBuilder
                                        .GetProperties(tElement)
                                        .Where(p => !p.IsFrameworkMember &&
                                                     p.IsVisible);

            var elementProperties = new List<PropertyVM>();
            foreach (var prop in allKnownProperties)
            {
                var propVM = _SearchFilterPropertyVmBuilder.BuildFor(prop);
                propVM.Index = elementProperties.Count;
                elementProperties.Add(propVM);
            }

            var result = new SearchResultsVM()
            {
                ID = oCollection.ID,
                Name = oCollection.Template.FriendlyName,
                Type = oCollection.Template.RealType.Name,
                ElementType = tElement.RealType.FullName,
                IsVisible = oCollection.Template.IsVisible,
                ElementProperties = elementProperties.ToArray(),
                Properties = this.CreateProperties(oCollection, allKnownProperties).ToArray(),
                Items = this.CreateItems(oCollection, allKnownProperties).ToArray(),
                AreMoreAvailable = searchResults.Count() > originalRequest.PageSize,

                DirtyState = this.CreateDirtyState(oCollection),
            };

            return result;
        }

        private IEnumerable<ObjectVM> CreateItems(CollectionObserver oCollection,
                                                  IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var items = new List<ObjectVM>();
            foreach (var obj in oCollection.GetItems())
            {
                var objType = _RealTypeResolver.GetRealType(obj);

                var oObject = (ObjectObserver)_ObserverCache.GetObserver(obj, objType);

                // Code smell: This is the only point where we know the persistent state:
                this.MarkAsPersistent(oObject);

                var objVM = this.BuildForObject(oObject, allKnownProperties);

                items.Add(objVM);
            }
            return items;
        }

        private void MarkAsPersistent(ObjectObserver oObject)
        {
            oObject.ChangeTracker.IsTransient = false;
        }

        private SearchResultItemVM BuildForObject(ObjectObserver oObject, IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var title = oObject.RealObject.ToString() ?? oObject.Template.FriendlyName;
            if (title == oObject.Template.FullName)
            {
                title = oObject.Template.FriendlyName;
            }

            var result = new SearchResultItemVM()
            {
                ID = oObject.ID,
                Name = title,
                Type = oObject.Template.RealType.Name,
                IsVisible = oObject.Template.IsVisible,
                IsPersistable = oObject.Template.IsPersistable,
                Properties = this.CreateProperties(oObject, allKnownProperties).ToArray(),
                DirtyState = this.CreateDirtyState(oObject),
            };

            return result;
        }

        private IEnumerable<PropertyVM> CreateProperties(ObjectObserver oObject, IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var properties = new List<PropertyVM>();
            foreach (var tProp in allKnownProperties)
            {
                var oProp = oObject.Properties.TryGetValueOrNull(tProp.Name);
                var propVM = oProp != null ?
                            _PropertyVmBuilder.BuildFor(oProp) :
                            _PropertyVmBuilder.BuildEmptyVMFor(tProp);
                propVM.Index = properties.Count;
                properties.Add(propVM);
            }
            return properties;
        }

        private DirtyStateVM CreateDirtyState(ObjectObserver oObject)
        {
            return new DirtyStateVM()
            {
                IsTransient = oObject.ChangeTracker.IsTransient,
                IsPersistent = oObject.ChangeTracker.IsPersistent,
                IsDirty = oObject.ChangeTracker.IsDirty,
                HasDirtyChildren = oObject.ChangeTracker.HasDirtyObjectGraph,
            };
        }
    }
}