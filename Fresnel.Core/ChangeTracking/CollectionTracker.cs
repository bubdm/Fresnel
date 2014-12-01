using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Tracks changes made to an ObjectObserverBase
    /// </summary>
    public class CollectionTracker : ObjectTracker
    {
        private Dictionary<Guid, ObjectObserver> _DirtyChildren = new Dictionary<Guid, ObjectObserver>();

        //private CollectionSnapshotTracker _CollectionSnapshotTracker;

        public CollectionTracker
            (
            OuterObjectsIdentifier outerObjectsIdentifier,
            CollectionObserver oCollection
            )
            : base(outerObjectsIdentifier, oCollection)
        {
        }

        //internal override void FinaliseConstruction()
        //{
        //    base.FinaliseConstruction();

        //    // TODO: Re-enable this:
        //    //_CollectionSnapshotTracker = new CollectionSnapshotTracker(_oObject.InnerCollection);
        //    //_CollectionSnapshotTracker.DetermineInitialState();

        //    //if (this.IsNewInstance)
        //    //{
        //    //    foreach (var oObj in _oObject.InnerCollection.GetInnerObjects())
        //    //    {
        //    //        oObj.ChangeTracker.DetermineInitialState();
        //    //    }
        //    //}
        //}

        public IEnumerable<ObjectObserver> DirtyChildren
        {
            get { return _DirtyChildren.Values; }
        }

        internal void MarkAsAdded(ObjectObserver oAddedItem)
        {
            _DirtyChildren[oAddedItem.ID] = oAddedItem;
        }

        internal void MarkAsRemoved(ObjectObserver oRemovedItem)
        {
            if (oRemovedItem.ChangeTracker.IsTransient)
            {
                _DirtyChildren.Remove(oRemovedItem.ID);
            }
            else
            {
                _DirtyChildren[oRemovedItem.ID] = oRemovedItem;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (_DirtyChildren.Any())
                    return true;

                return base.IsDirty;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public override void DetectChanges()
        {
            // Optimisation: Don't bother taking a snapshot if the value is already dirty:
            if (this.IsDirty)
                return;

            base.DetectChanges();

            // TODO: Re-enable this:

            //if (_CollectionSnapshotTracker != null &&
            //    _CollectionSnapshotTracker.DetectChanges().Passed)
            //{
            //    // NB: Use the property setter, so that the dirty status is cascaded:
            //    this.HasChanges = true;
            //}
        }

        public override void Dispose()
        {
            _DirtyChildren.Clear();
            _DirtyChildren = null;

            // TODO: Re-enable this:
            //_CollectionSnapshotTracker.DisposeSafely();
            //_CollectionSnapshotTracker = null;

            base.Dispose();
        }

    }
}
