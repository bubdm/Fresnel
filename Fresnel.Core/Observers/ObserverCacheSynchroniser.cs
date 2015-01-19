using Envivo.Fresnel.Introspection;
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
            // We may have accessed a Collection property, but haven't
            var knownObservers = ObserverCache.GetAllObservers().ToArray();
            foreach (var oObject in knownObservers)
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

            // Now we can scan:
            foreach (var oObject in this.ObserverCache.GetAllObservers())
            {
                this.Sync(oObject);
            }
        }

        public void Sync(ObjectObserver oObject)
        {
            foreach (var oProp in oObject.Properties.Values.OfType<ObjectPropertyObserver>())
            {
                if (oProp.Template.IsReferenceType)
                    continue;

                var value = oProp.Template.GetProperty(oObject.RealObject);
                var valueType = _RealTypeResolver.GetRealType(value);
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
                var oPreviousValue = this.ObserverCache.GetObserver(oProperty.PreviousValue);

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