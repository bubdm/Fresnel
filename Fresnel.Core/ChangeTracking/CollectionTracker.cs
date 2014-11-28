using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Tracks changes made to an ObjectObserverBase
    /// </summary>
    public class CollectionTracker : ObjectTracker
    {
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
            // TODO: Re-enable this:
            //_CollectionSnapshotTracker.DisposeSafely();
            //_CollectionSnapshotTracker = null;

            base.Dispose();
        }

    }
}
