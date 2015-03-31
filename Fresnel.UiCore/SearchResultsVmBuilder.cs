using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Linq;

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

        public SearchResultsVM BuildForCollection(CollectionObserver oCollection, ClassTemplate tElement)
        {
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
                Properties = this.CreateProperties(oCollection).ToArray(),
                Items = this.CreateItems(oCollection, allKnownProperties).ToArray(),
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
                var objVM = this.BuildForObject(oObject);

                items.Add(objVM);
            }
            return items;
        }

        private SearchResultItemVM BuildForObject(ObjectObserver oObject)
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
                Properties = this.CreateProperties(oObject).ToArray(),
                DirtyState = this.CreateDirtyState(oObject),
            };

            return result;
        }

        private IEnumerable<PropertyVM> CreateProperties(ObjectObserver oObject)
        {
            var visibleProperties = oObject.Properties.Values.Where(p => !p.Template.IsFrameworkMember &&
                                                                          p.Template.IsVisible);

            var properties = new List<PropertyVM>();
            foreach (var oProp in visibleProperties)
            {
                var propVM = _PropertyVmBuilder.BuildFor(oProp);
                propVM.Description = null;
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