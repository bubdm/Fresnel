using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class RemoveFromCollectionEvent : IFresnelEvent
    {
        public RemoveFromCollectionEvent
            (
            ObjectPropertyObserver collectionProperty,
            CollectionObserver oCollection,
            ObjectObserver oItemToRemove
            )
        {
            this.CollectionProperty = collectionProperty;
            this.Collection = oCollection;
            this.RemovedItem = oItemToRemove;

            this.AffectedObjects = new ObjectObserver[] { oCollection, oItemToRemove};
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public ObjectPropertyObserver CollectionProperty { get; set; }
        
        public CollectionObserver Collection { get; set; }
        
        public ObjectObserver RemovedItem { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }
    }
}