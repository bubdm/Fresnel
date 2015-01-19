using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An Observer for a Domain Object Collection
    /// </summary>
    public class CollectionObserver : ObjectObserver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="collection">The Collection object</param>
        /// <param name="collectionType">The Type of the Collection</param>

        public CollectionObserver
        (
            object collection,
            Type collectionType,
            CollectionTemplate tCollection,
            PropertyObserverMapBuilder propertyObserverMapBuilder,
            MethodObserverMapBuilder methodObserverMapBuilder,
            AbstractChangeTrackerBuilder changeTrackerBuilder
        )
            : base(collection, collectionType, tCollection, propertyObserverMapBuilder, methodObserverMapBuilder, changeTrackerBuilder)
        {
        }

        [JsonIgnore]
        public new CollectionTemplate Template
        {
            get { return (CollectionTemplate)base.Template; }
        }

        public new CollectionTracker ChangeTracker
        {
            get { return (CollectionTracker)base.ChangeTracker; }
        }

        /// <summary>
        /// Ensures that all Domain Objects know that they belong to this Collection
        /// </summary>
        internal void BindItemsToCollection(IEnumerable<ObjectObserver> items)
        {
            foreach (var oItem in items)
            {
                oItem.AssociateWith(this);
            }
        }

        /// <summary>
        /// Performs the inverse of <see>BindItemsToCollection</see>
        /// </summary>
        internal void UnbindItemsFromCollection(IEnumerable<ObjectObserver> items)
        {
            foreach (var oItem in items)
            {
                oItem.DisassociateFrom(this);
            }
        }

        /// <summary>
        /// Returns an enumerable of the underlying Collection objects (not observers)
        /// </summary>
        public IEnumerable<object> GetItems()
        {
            var items = this.RealObject as IEnumerable;
            return items.Cast<object>();
        }

        public IEnumerable<object> PreviousItems { get; internal set; }

        public override void Dispose()
        {
            base.Dispose();
            this.PreviousItems = null;
        }
    }
}