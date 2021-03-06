using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class CollectionVmBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ObserverRetriever _ObserverRetriever;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private EmptyPropertyVmBuilder _EmptyPropertyVmBuilder;
        private PropertyVmBuilder _PropertyVmBuilder;
        private MethodVmBuilder _MethodVmBuilder;
        private ObjectVmBuilder _ObjectVmBuilder;
        private DirtyStateVmBuilder _DirtyStateVmBuilder;

        private CollectionInteractionsBuilder _CollectionInteractionsBuilder;

        public CollectionVmBuilder
            (
            RealTypeResolver realTypeResolver,
            ObserverRetriever observerRetriever,
            ClassHierarchyBuilder classHierarchyBuilder,
            EmptyPropertyVmBuilder emptyPropertyVmBuilder,
            PropertyVmBuilder propertyVmBuilder,
            MethodVmBuilder methodVmBuilder,
            ObjectVmBuilder objectVmBuilder,
            DirtyStateVmBuilder dirtyStateVmBuilder,
            CollectionInteractionsBuilder collectionInteractionsBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ObserverRetriever = observerRetriever;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _EmptyPropertyVmBuilder = emptyPropertyVmBuilder;
            _PropertyVmBuilder = propertyVmBuilder;
            _MethodVmBuilder = methodVmBuilder;
            _ObjectVmBuilder = objectVmBuilder;
            _DirtyStateVmBuilder = dirtyStateVmBuilder;
            _CollectionInteractionsBuilder = collectionInteractionsBuilder;
        }

        public ObjectVM BuildFor(CollectionObserver oCollection)
        {
            // The View needs to know about ALL properties for all sub-classes of the Collection's element type:
            var tElement = oCollection.Template.InnerClass;
            var result = this.BuildFor(oCollection, tElement);

            result.AllowedClassTypes = _CollectionInteractionsBuilder.BuildElementTypesFor(oCollection);

            return result;
        }

        public ObjectVM BuildFor(ObjectPropertyObserver oCollectionProperty, CollectionObserver oCollection)
        {
            // The View needs to know about ALL properties for all sub-classes of the Collection's element type:
            var tElement = oCollection.Template.InnerClass;
            var result = this.BuildFor(oCollection, tElement);

            result.Add = _CollectionInteractionsBuilder.BuildAdd(oCollectionProperty, oCollection);
            result.AllowedClassTypes = _CollectionInteractionsBuilder.BuildElementTypesFor(oCollection);

            return result;
        }
        
        private CollectionVM BuildFor(CollectionObserver oCollection, ClassTemplate tElement)
        {
            var tSubClasses = _ClassHierarchyBuilder.GetSubClasses(tElement, false, false).ToArray();
            var tSubClassNames = tSubClasses.Any() ?
                                    tSubClasses.Select(t => t.FullName).ToArray() :
                                    null;

            var allKnownProperties = _ClassHierarchyBuilder
                                        .GetProperties(tElement)
                                        .Where(p => !p.IsFrameworkMember &&
                                                     p.IsVisible);

            var elementProperties = new List<PropertyVM>();
            foreach (var tProp in allKnownProperties)
            {
                var propVM = _EmptyPropertyVmBuilder.BuildFor(tProp);
                propVM.Index = elementProperties.Count;
                elementProperties.Add(propVM);
            }

            // We'll reuse the ObjectVmBuilder for the basic stuff:
            var objectVM = _ObjectVmBuilder.BuildFor(oCollection, allKnownProperties);

            var result = new CollectionVM()
            {
                ID = objectVM.ID,
                Name = objectVM.Name,
                Type = objectVM.Type,
                ElementType = tElement.RealType.FullName,
                IsVisible = objectVM.IsVisible,
                ElementProperties = elementProperties.ToArray(),
                Description = objectVM.Description,
                Properties = objectVM.Properties,
                Methods = objectVM.Methods,
                Items = this.CreateItems(oCollection, allKnownProperties).ToArray(),

                DirtyState = _DirtyStateVmBuilder.BuildFor(oCollection),
            };

            this.TrimRedundantContentFrom(result);

            return result;
        }

        private IEnumerable<ObjectVM> CreateItems(CollectionObserver oCollection, IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var items = new List<ObjectVM>();
            foreach (var obj in oCollection.GetItems())
            {
                var objType = _RealTypeResolver.GetRealType(obj);

                var oObject = (ObjectObserver)_ObserverRetriever.GetObserver(obj, objType);
                var objVM = _ObjectVmBuilder.BuildFor(oObject, allKnownProperties);

                items.Add(objVM);
            }
            return items;
        }

        private void TrimRedundantContentFrom(CollectionVM collectionVM)
        {
            foreach (var item in collectionVM.Items)
            {
                item.Description = null;
                foreach (var prop in item.Properties)
                {
                    prop.Description = null;
                }
                foreach (var method in item.Methods)
                {
                    method.Description = null;
                }
            }
        }
    }
}