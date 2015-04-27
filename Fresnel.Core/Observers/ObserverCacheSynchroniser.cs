using Envivo.Fresnel.Introspection;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// Synchronises all known Observers with the underlying domain object graph
    /// </summary>
    public class ObserverCacheSynchroniser
    {
        private RealTypeResolver _RealTypeResolver;

        public ObserverCacheSynchroniser
            (
            RealTypeResolver realTypeResolver
            )
        {
            _RealTypeResolver = realTypeResolver;
        }

        public ObserverCache ObserverCache { get; set; }

        public void SyncAll()
        {
            // Make sure we have a known state *before* we scan the observers for changes
            // (after all, we don't want to detect false Collection modifications:
            this.EnsureCacheIsHasKnownState();

            // This prevents the ORM from accidentally triggering load operations:
            var observersToSync = this.GetObserversThatWontTriggerLazyLoads();

            // Now we can scan:
            foreach (var oObject in observersToSync)
            {
                this.Sync(oObject);
            }
        }

        private void EnsureCacheIsHasKnownState()
        {
            var allObservers = this.ObserverCache.GetAllObservers().ToArray();
            foreach (var oObject in allObservers)
            {
                var outerObjectProperties = oObject.OuterProperties.OfType<ObjectPropertyObserver>();
                var collectionProperties = outerObjectProperties.Where(p => p.Template.IsCollection);
                if (collectionProperties.Any(p => p.IsLazyLoaded))
                {
                    var items = ((CollectionObserver)oObject).GetItems();
                    foreach (var item in items)
                    {
                        var itemType = _RealTypeResolver.GetRealType(item);
                        var oItem = this.ObserverCache.GetObserver(item, itemType);
                    }
                }
            }
        }

        public ObjectObserver[] GetObserversThatWontTriggerLazyLoads()
        {
            var results = new List<ObjectObserver>();

            var allObservers = this.ObserverCache.GetAllObservers().ToList();
            foreach (var oObject in allObservers)
            {
                var outerObjectProperties = oObject.OuterProperties.OfType<ObjectPropertyObserver>();
                var referenceProperties = outerObjectProperties.Where(p => p.Template.IsReferenceType);

                var isStandAloneObject = !outerObjectProperties.Any();
                var hasBeenLazyLoaded = referenceProperties.Any(p => p.IsLazyLoaded);

                if (isStandAloneObject || hasBeenLazyLoaded)
                {
                    results.Add(oObject);
                }
                else if (!hasBeenLazyLoaded)
                {
                    // We don't want to check this object
                }
            }

            return results.ToArray();
        }

        public void Sync(ObjectObserver oObject)
        {
            foreach (var oProp in oObject.Properties.Values.OfType<ObjectPropertyObserver>())
            {
                if (oProp.Template.IsNonReference)
                    continue;

                if (oProp.IsLazyLoadPending)
                    continue;

                var value = oProp.Template.GetProperty(oObject.RealObject);
                var valueType = value != null ?
                                _RealTypeResolver.GetRealType(value) :
                                oProp.Template.PropertyType;

                var oValue = this.ObserverCache.GetObserver(value, valueType);

                this.Sync(oProp, oValue);
            }

            var oCollection = oObject as CollectionObserver;
            if (oCollection != null)
            {
                this.Sync(oCollection);
            }
        }

        public void Sync(CollectionObserver oCollection)
        {
            var previousItems = oCollection.PreviousItems ?? new object[0];
            var latestItems = oCollection.GetItems();

            var addedItems = latestItems.Except(previousItems).ToArray();
            var removedItems = previousItems.Except(latestItems).ToArray();

            var isDifferent = addedItems.Any() || removedItems.Any();
            if (isDifferent)
            {
                foreach (var item in removedItems)
                {
                    var itemType = _RealTypeResolver.GetRealType(item);
                    var oItem = ObserverCache.GetObserver(item, itemType);
                    oItem.DisassociateFrom(oCollection);
                }

                foreach (var item in addedItems)
                {
                    var itemType = _RealTypeResolver.GetRealType(item);
                    var oItem = ObserverCache.GetObserver(item, itemType);
                    oItem.AssociateWith(oCollection);
                }

                oCollection.PreviousItems = latestItems;
            }
        }

        public void Sync(ObjectPropertyObserver oProperty, BaseObjectObserver oValue)
        {
            if (oProperty == null)
                return;

            if (oProperty.IsLazyLoadPending)
                return;

            if (oProperty.PreviousValue != null)
            {
                var itemType = _RealTypeResolver.GetRealType(oProperty.PreviousValue);
                var oPreviousValue = this.ObserverCache.GetObserver(oProperty.PreviousValue, itemType);

                var isDifferent = !(object.Equals(oValue.RealObject, oPreviousValue.RealObject));
                if (isDifferent)
                {
                    // Make the object aware that it is associated with this property:
                    oPreviousValue.DisassociateFrom(oProperty);
                }
            }
            else
            {
                oValue.AssociateWith(oProperty);
            }

            oValue.AssociateWith(oProperty);
            oProperty.PreviousValue = oValue.RealObject;

            // Make sure the contents are synced too:
            if (oProperty.Template.IsCollection)
            {
                var oCollection = (CollectionObserver)oValue;
                this.Sync(oCollection);
            }
        }
    }
}