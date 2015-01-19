using Envivo.Fresnel.Core.Observers;
using System;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public class AbstractChangeTrackerBuilder
    {
        private Func<ObjectObserver, ObjectTracker> _ObjectTrackerFactory;
        private Func<CollectionObserver, CollectionTracker> _CollectionTrackerFactory;

        public AbstractChangeTrackerBuilder
            (
            Func<ObjectObserver, ObjectTracker> objectTrackerFactory,
            Func<CollectionObserver, CollectionTracker> collectionTrackerFactory
            )
        {
            _ObjectTrackerFactory = objectTrackerFactory;
            _CollectionTrackerFactory = collectionTrackerFactory;
        }

        public IChangeTracker BuildFor(ObjectObserver oObject)
        {
            var oCollection = oObject as CollectionObserver;

            var result = oCollection != null ?
                        _CollectionTrackerFactory(oCollection) :
                        _ObjectTrackerFactory(oObject);

            result.FinaliseConstruction();

            return result;
        }
    }
}