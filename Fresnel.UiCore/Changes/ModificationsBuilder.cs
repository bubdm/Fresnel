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

        public Modifications BuildFrom(SessionJournal sessionJournal, long startedAt)
        {
            var newObjects = sessionJournal.ObjectCreations.SkipWhile(o => o.Sequence < startedAt);
            var propertyChanges = sessionJournal.PropertyChanges.SkipWhile(p => p.Sequence < startedAt);
            var collectionAdds = sessionJournal.CollectionAdditions.SkipWhile(p => p.Sequence < startedAt);
            var collectionRemoves = sessionJournal.CollectionRemovals.SkipWhile(p => p.Sequence < startedAt);

            var result = new Modifications()
            {
                NewObjects = newObjects.Select(o => _AbstractObjectVMBuilder.BuildFor(o.Object)).ToArray(),
                PropertyChanges = propertyChanges.Select(p => CreatePropertyChange(p.Property)).ToArray(),
                CollectionAdditions = collectionAdds.Select(c => CreateCollectionElement(c.Collection, c.Element)).ToArray(),
                CollectionRemovals = collectionRemoves.Select(c => CreateCollectionElement(c.Collection, c.Element)).ToArray(),
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
