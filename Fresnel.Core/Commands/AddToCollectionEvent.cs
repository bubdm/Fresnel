using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class AddToCollectionEvent : IFresnelEvent
    {
        public AddToCollectionEvent
            (
            ObjectPropertyObserver collectionProperty,
            CollectionObserver oCollection,
            ObjectObserver addedItem
            )
        {
            this.CollectionProperty = collectionProperty;
            this.Collection = oCollection;
            this.AddedItem = addedItem;

            this.AffectedObjects = new ObjectObserver[] { oCollection, addedItem };
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public ObjectPropertyObserver CollectionProperty { get; set; }

        public CollectionObserver Collection { get; set; }

        public ObjectObserver AddedItem { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

    }
}