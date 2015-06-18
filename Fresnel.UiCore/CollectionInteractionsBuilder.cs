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

        public MethodVM[] BuildElementFactoryMethods(CollectionObserver oCollection)
        {
            var tElement = oCollection.Template.InnerClass;
            var results = this.BuildFactoryMethods(tElement);
            return results.ToArray();
        }

        public MethodVM[] BuildElementSubClassFactoryMethods(CollectionObserver oCollection)
        {
            var tSubClasses = _ClassHierarchyBuilder.GetSubClasses(oCollection.Template.InnerClass, false, true);
            if (!tSubClasses.Any())
                return null;

            var results = new List<MethodVM>();
            foreach (var tSubClass in tSubClasses)
            {
                results.AddRange(this.BuildFactoryMethods(tSubClass));
            }

            return results.ToArray();
        }

        private IEnumerable<MethodVM> BuildFactoryMethods(ClassTemplate tSubClass)
        {
            var results = new List<MethodVM>();

            var classItemVm = _ClassItemBuilder.BuildFor(tSubClass);

            if (classItemVm.IsVisible)
            {
                var defaultCreate = new MethodVM
                {
                    Name = classItemVm.Name,
                    InternalName = tSubClass.FullName,
                    IsVisible = classItemVm.Create.IsVisible,
                    IsEnabled = classItemVm.Create.IsEnabled,
                    Error = classItemVm.Create.Error
                };
                results.Add(defaultCreate);
            }

            if (classItemVm.FactoryMethods != null)
            {
                results.AddRange(classItemVm.FactoryMethods);
            }

            return results;
        }

    }
}