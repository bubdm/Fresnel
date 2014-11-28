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
        //private Dictionary<Guid, ObjectObserver> _oDirtyChildren = new Dictionary<Guid, ObjectObserver>();
        private Dictionary<Guid, ObjectObserver> _oPreviousOwners = new Dictionary<Guid, ObjectObserver>();

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
        public bool IsDirty
        {
            get
            {
                if (_HasLocalChanges)
                    return true;

                if (this.IsMarkedForRemoval && this.IsNewInstance && this.IsMarkedForAddition == false)
                    return false;

                if (this.IsMarkedForAddition)
                    return true;

                if (this.IsMarkedForRemoval)
                    return true;

                if (this.IsNewInstance)
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
        public bool IsNewInstance { get; set; }

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
        public bool IsMarkedForAddition { get; set; }

        /// <summary>
        /// Determines if the associated Object is marked for deletion
        /// </summary>
        public bool IsMarkedForRemoval { get; set; }

        /// <summary>
        /// Returns TRUE if the Object Graph has any objects that are dirty.
        /// This eliminates the need to scan the entire graph every time we want to check if anything is dirty.
        /// </summary>
        public bool HasDirtyObjectGraph
        {
            get { return (_oDirtyObjectGraph.Count > 0); }
        }

        ///// <summary>
        ///// Returns TRUE if the associated Object has immediate dirty children.
        ///// This eliminates the need to scan the entire graph every time we want to check if the Object is dirty.
        ///// </summary>
        //public bool HasDirtyChildren
        //{
        //    get { return (_oDirtyChildren.Count > 0); }
        //}

        /// <summary>
        /// A list of all dirty objects that are within the connected object graph
        /// </summary>
        public IEnumerable<ObjectObserver> DirtyObjectGraph
        {
            get { return _oDirtyObjectGraph.Values; }
        }

        ///// <summary>
        ///// A list of all immediate dirty child Domain Objects
        ///// </summary>
        //public IEnumerable<ObjectObserver> DirtyChildren
        //{
        //    get { return _oDirtyChildren.Values; }
        //}


        private void AddToDirtyObjectGraph(ObjectObserver oObject)
        {
            if (oObject.Template.IsPersistable == false)
                return;

            if (_oDirtyObjectGraph.Contains(oObject.ID))
                return;

            //System.Diagnostics.Debug.WriteLine("Dirty graph add : " + oObject.DebugID + " to " + _oObject.DebugID);
            _oDirtyObjectGraph.Add(oObject.ID, oObject);
        }

        internal void RemoveFromDirtyObjectGraph(ObjectObserver oObject)
        {
            if (oObject.Template.IsPersistable == false)
                return;

            if (_oDirtyObjectGraph.DoesNotContain(oObject.ID))
                return;

            //System.Diagnostics.Debug.WriteLine("Dirty graph remove : " + oObject.DebugID + " from " + _oObject.DebugID);
            _oDirtyObjectGraph.Remove(oObject.ID);
        }

        /// <summary>
        /// Records which collection the Object was removed from (it will be needed when resetting dirty object graphs)
        /// </summary>
        /// <param name="oCollection"></param>
        internal void MarkForRemovalFrom(CollectionObserver oCollection)
        {
            this.IsMarkedForRemoval = true;

            if (_oPreviousOwners.Contains(oCollection.ID))
                return;

            _oPreviousOwners.Add(oCollection.ID, oCollection);
        }

        /// <summary>
        /// Resets the dirty flag for this Object and cascades the status to all parents
        /// </summary>
        /// <remarks></remarks>
        internal void ResetDirtyFlags()
        {
            this.IsNewInstance = false;
            this.IsDirty = false;
            this.IsMarkedForAddition = false;

            if (this.IsMarkedForRemoval)
            {
                this.IsMarkedForRemoval = false;
            }

            //_ObjectSnapshotTracker.Reset();
        }

        internal void AddDirtyObject(ObjectObserver oDirtyObject)
        {
            _oDirtyObjectGraph.Add(oDirtyObject.ID, oDirtyObject);
        }

        public bool CanDispose
        {
            get { return this.IsDirty == false; }
        }

        public virtual void Dispose()
        {
            _oObject = null;

            //_oDirtyChildren.ClearSafely();
            //_oDirtyChildren = null;

            _oDirtyObjectGraph.ClearSafely();
            _oDirtyObjectGraph = null;

            //_ObjectSnapshotTracker.DisposeSafely();
            //_ObjectSnapshotTracker = null;

            //_EventChangeTracker.DisposeSafely();
            //_EventChangeTracker = null;
        }

    }
}
