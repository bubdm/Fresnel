using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Tracks changes made to an ObjectObserverBase
    /// </summary>
    public class ObjectTracker : IChangeTracker
    {
        private OuterObjectsIdentifier _OuterObjectsIdentifier;
        private ObjectObserver _oObject;
        //private ObjectSnapshotTracker _ObjectSnapshotTracker;
        //private EventBoundChangeTracker _EventChangeTracker;

        private Dictionary<Guid, ObjectObserver> _oDirtyObjectGraph = new Dictionary<Guid, ObjectObserver>();
        private Dictionary<Guid, ObjectObserver> _oPreviousParents = new Dictionary<Guid, ObjectObserver>();
        private Dictionary<Guid, ObjectObserver> _oNewParents = new Dictionary<Guid, ObjectObserver>();

        private bool _HasLocalChanges = false;

        public ObjectTracker
            (
            OuterObjectsIdentifier outerObjectsIdentifier,
            ObjectObserver oObject
            )
        {
            _OuterObjectsIdentifier = outerObjectsIdentifier;
            _oObject = oObject;
        }

        internal virtual void FinaliseConstruction()
        {
            var tClass = _oObject.Template;

            if (tClass.RealType.IsNonReference())
                return;

            //_ObjectSnapshotTracker = new ObjectSnapshotTracker(_oObject.InnerObject);
            //_ObjectSnapshotTracker.DetermineInitialState();
        }

        /// <summary>
        /// Determines if the associated Object is dirty. This does NOT include associated dirty objects.
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {
                if (_HasLocalChanges)
                    return true;

                if (this.IsMarkedForAddition)
                    return true;

                if (this.IsMarkedForRemoval)
                    return true;

                if (this.IsTransient)
                    return true;

                return false;
            }
            set
            {
                _HasLocalChanges = value;
            }
        }

        /// <summary>
        /// Determines if the associated Object is a brand new instance, and doesn't exist in the Repository
        /// </summary>
        public bool IsTransient { get; set; }

        public bool IsPersistent
        {
            get { return !this.IsTransient; }
        }

        public virtual void DetectChanges()
        {
            // Optimisation: Don't bother taking a snapshot if the value is already dirty:
            if (this.IsDirty)
                return;

            //if (_ObjectSnapshotTracker != null &&
            //    _ObjectSnapshotTracker.DetectChanges().Passed)
            //{
            //    // NB: Use the property setter, so that the dirty status is cascaded:
            //    this.HasChanges = true;
            //}

            //if (_CollectionSnapshotTracker != null &&
            //    _CollectionSnapshotTracker.DetectChanges().Passed)
            //{
            //    // NB: Use the property setter, so that the dirty status is cascaded:
            //    this.HasChanges = true;
            //}
        }

        internal void DetectChangesSince(DateTime timeStampUtc)
        {
            var oObject = _oObject as ObjectObserver;
            if (oObject == null)
                return;

            // TODO: Re-enable this:
            //foreach (var oProp in oObject.Properties.Values)
            //{
            //    if (oProp.LastUpdatedAtUtc > timeStampUtc)
            //    {
            //        oProp.InnerObserver.ChangeTracker.DetectChanges();
            //    }
            //}
        }

        /// <summary>
        /// Determines if the associated Object is marked for insertion
        /// </summary>
        public bool IsMarkedForAddition
        {
            get { return _oNewParents.Any(); }
        }

        /// <summary>
        /// Determines if the associated Object is marked for deletion
        /// </summary>
        public bool IsMarkedForRemoval
        {
            get { return _oPreviousParents.Any(); }
        }

        /// <summary>
        /// Returns TRUE if the Object Graph has any objects that are dirty.
        /// This eliminates the need to scan the entire graph every time we want to check if anything is dirty.
        /// </summary>
        public bool HasDirtyObjectGraph
        {
            get { return (_oDirtyObjectGraph.Count > 0); }
        }

        /// <summary>
        /// A list of all dirty objects that are within the connected object graph
        /// </summary>
        public IEnumerable<ObjectObserver> DirtyObjectGraph
        {
            get { return _oDirtyObjectGraph.Values; }
        }

        private void AddToDirtyObjectGraph(ObjectObserver oObject)
        {
            //System.Diagnostics.Debug.WriteLine("Dirty graph add : " + oObject.DebugID + " to " + _oObject.DebugID);
            _oDirtyObjectGraph[oObject.ID] = oObject;
        }

        internal void RemoveFromDirtyObjectGraph(ObjectObserver oObject)
        {
            //System.Diagnostics.Debug.WriteLine("Dirty graph remove : " + oObject.DebugID + " from " + _oObject.DebugID);
            _oDirtyObjectGraph.Remove(oObject.ID);
        }

        /// <summary>
        /// Records which collection the Object was added to (it will be needed when resetting dirty object graphs)
        /// </summary>
        /// <param name="oCollection"></param>
        internal void MarkForAdditionTo(CollectionObserver oCollection)
        {
            _oNewParents[oCollection.ID] = oCollection;
        }

        /// <summary>
        /// Records which collection the Object was removed from (it will be needed when resetting dirty object graphs)
        /// </summary>
        /// <param name="oCollection"></param>
        internal void MarkForRemovalFrom(CollectionObserver oCollection)
        {
            if (this.IsTransient)
            {
                _oNewParents.Remove(oCollection.ID);
            }
            else
            {
                // If the item was previously added and now it's removed, the collection is effectively unchanged:
                _oPreviousParents[oCollection.ID] = oCollection;
            }
        }

        /// <summary>
        /// Resets the dirty flag for this Object and cascades the status to all parents
        /// </summary>
        /// <remarks></remarks>
        internal void ResetDirtyFlags()
        {
            this.IsTransient = false;
            this.IsDirty = false;

            _oNewParents.Clear();
            _oPreviousParents.Clear();
        }

        /// <summary>
        /// Adds the given dirty object to the object graph. Note that the object may not be an immediate child.
        /// </summary>
        /// <param name="oDirtyObject"></param>
        internal void AddDirtyObject(ObjectObserver oDirtyObject)
        {
            if (_oDirtyObjectGraph.ContainsKey(oDirtyObject.ID))
                return;

            _oDirtyObjectGraph[oDirtyObject.ID] = oDirtyObject;
        }

        public bool CanDispose
        {
            get { return this.IsDirty == false; }
        }

        public virtual void Dispose()
        {
            _oObject = null;

            _oDirtyObjectGraph.ClearSafely();
            _oDirtyObjectGraph = null;

            //_ObjectSnapshotTracker.DisposeSafely();
            //_ObjectSnapshotTracker = null;

            //_EventChangeTracker.DisposeSafely();
            //_EventChangeTracker = null;
        }

    }
}
