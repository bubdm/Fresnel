using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.UiCore.Changes
{
    public class ModificationsBuilder
    {
        private AbstractObjectVMBuilder _AbstractObjectVMBuilder;
        private AbstractPropertyVmBuilder _AbstractPropertyVmBuilder;
        private ObjectIdResolver _ObjectIdResolver;

        public ModificationsBuilder
            (
            AbstractObjectVMBuilder abstractObjectVMBuilder,
            AbstractPropertyVmBuilder abstractPropertyVmBuilder,
            ObjectIdResolver objectIdResolver
            )
        {
            _AbstractObjectVMBuilder = abstractObjectVMBuilder;
            _AbstractPropertyVmBuilder = abstractPropertyVmBuilder;
            _ObjectIdResolver = objectIdResolver;
        }

        public Modifications BuildFrom(IEnumerable<ObjectObserver> observers, long startedAt)
        {
            //var newObjects = changeLog.NewObjects.SkipWhile(o => o.Sequence < startedAt).ToArray();
            var propertyChanges = observers.SelectMany(o => o.ChangeTracker.GetPropertyChangesSince(startedAt)).ToArray();
            //var collectionAdds = changeLog.CollectionAdditions.SkipWhile(p => p.Sequence < startedAt).ToArray();
            //var collectionRemoves = changeLog.CollectionRemovals.SkipWhile(p => p.Sequence < startedAt).ToArray();

            var result = new Modifications()
            {
                //NewObjects = newObjects.Select(o => _AbstractObjectVMBuilder.BuildFor(o.Object)),
                PropertyChanges = propertyChanges.Select(c => CreatePropertyChange(c)),
                //CollectionAdditions = collectionAdds.Select(c => CreateCollectionElement(c.Collection, c.Element)),
                //CollectionRemovals = collectionRemoves.Select(c => CreateCollectionElement(c.Collection, c.Element)),
            };

            return result;
        }

        private PropertyChangeVM CreatePropertyChange(PropertyChange propertyChange)
        {
            var oProperty = propertyChange.Property;

            var result = new PropertyChangeVM()
            {
                ObjectId = oProperty.OuterObject.ID,
                PropertyName = oProperty.Template.Name,
            };

            if (oProperty.Template.IsNonReference)
            {
                result.NonReferenceValue = propertyChange.NewValue;
            }
            else
            {
                result.ReferenceValueId = _ObjectIdResolver.GetId(propertyChange.NewValue, (ClassTemplate)oProperty.Template.InnerClass);
            }

            return result;
        }

        private CollectionElementVM CreateCollectionElement(CollectionObserver oCollection, ObjectObserver oElement)
        {
            var result = new CollectionElementVM()
            {
                CollectionId = oCollection.ID,
                ElementId = oElement.ID,
            };
            return result;
        }

    }
}
