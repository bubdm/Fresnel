using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Classes;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class CollectionInteractionsBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private ClassItemBuilder _ClassItemBuilder;

        public CollectionInteractionsBuilder
            (
            RealTypeResolver realTypeResolver,
            ClassHierarchyBuilder classHierarchyBuilder,
            ClassItemBuilder classItemBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _ClassItemBuilder = classItemBuilder;
        }

        public InteractionPoint BuildAdd(ObjectPropertyObserver oCollectionProperty, CollectionObserver oCollection)
        {
            var result = new InteractionPoint
            {
                IsVisible = oCollectionProperty.Template.IsAggregateRelationship,
                IsEnabled = true //TODO: Check CanAddToCollection permission here
            };

            return result;
        }

        public ClassItem[] BuildElementTypesFor(CollectionObserver oCollection)
        {
            var tSubClasses = _ClassHierarchyBuilder.GetSubClasses(oCollection.Template.InnerClass, false, true);
            if (!tSubClasses.Any())
                return null;

            var results = new List<ClassItem>();
            foreach (var tSubClass in tSubClasses)
            {
                var classItemVm = _ClassItemBuilder.BuildFor(tSubClass);
                results.Add(classItemVm);
            }

            return results.ToArray();
        }
    }
}