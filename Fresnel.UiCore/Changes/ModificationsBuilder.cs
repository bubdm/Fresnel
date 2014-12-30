using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Proxies.ChangeTracking;
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

        public Modifications BuildFrom(ChangeLog changeLog, long startedAt)
        {
            var newObjects = changeLog.ObjectCreations.SkipWhile(o => o.Sequence < startedAt).ToArray();
            var propertyChanges = changeLog.PropertyChanges.SkipWhile(p => p.Sequence < startedAt).ToArray();
            var collectionAdds = changeLog.CollectionAdditions.SkipWhile(p => p.Sequence < startedAt).ToArray();
            var collectionRemoves = changeLog.CollectionRemovals.SkipWhile(p => p.Sequence < startedAt).ToArray();

            var result = new Modifications()
            {
                NewObjects = newObjects.Select(o => _AbstractObjectVMBuilder.BuildFor(o.Object)),
                PropertyChanges = propertyChanges.Select(p => CreatePropertyChange(p.Property)),
                CollectionAdditions = collectionAdds.Select(c => CreateCollectionElement(c.Collection, c.Element)),
                CollectionRemovals = collectionRemoves.Select(c => CreateCollectionElement(c.Collection, c.Element)),
            };

            return result;
        }

        private PropertyChangeVM CreatePropertyChange(BasePropertyObserver oProperty)
        {
            var result = new PropertyChangeVM()
            {
                ObjectId = oProperty.OuterObject.ID,
                PropertyName = oProperty.Template.Name,

            };
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
