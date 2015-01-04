using System;
using System.Collections;
using System.Collections.Generic;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Core.ChangeTracking;
using Newtonsoft.Json;

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

        //public CollectionTemplate CollectionTemplate
        //{
        //    get { return (CollectionTemplate)base.Template; }
        //}

        /// <summary>
        /// Returns an Enumerator of the underlying Collection
        /// </summary>
        public IEnumerable GetContents()
        {
            return this.RealObject as IEnumerable;
        }

        ///// <summary>
        ///// Returns a pseudo checksum for all items in the Collection
        ///// </summary>

        //internal ulong CalculateChecksum()
        //{
        //    ulong result = 0;
        //    foreach (var item in this.GetContents())
        //    {
        //        // Use XOR and shift to create a pseudo-unique value:
        //        result ^= (ulong)item.GetHashCode();
        //        result = result << 1;
        //    }

        //    return result;
        //}

        public IEnumerable<ObjectObserver> PreviousContents { get; internal set; }

        //private void UpdatePersistenceForRemovedItem(ObjectObserver oRemovedItem)
        //{
        //    if (oRemovedItem.IsPersistable == false)
        //        return;

        //    oRemovedItem.ChangeTracker.IsMarkedForRemoval = true;

        //    if (oRemovedItem.ChangeTracker.IsNewInstance)
        //    {
        //        // This prevents unsaved instances remaining linked to this collection:
        //        this.ChangeTracker.RemoveFromDirtyObjectGraph(oRemovedItem);
        //        this.ChangeTracker.DirtyChildren.Remove(oRemovedItem);
        //    }
        //    else
        //    {
        //        oRemovedItem.ChangeTracker.MarkForRemovalFrom(this);
        //    }
        //}

        public override void Dispose()
        {
            base.Dispose();
            this.PreviousContents = null;
        }

    }
}
