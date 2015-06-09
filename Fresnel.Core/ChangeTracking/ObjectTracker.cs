using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Tracks changes made to an ObjectObserverBase
    /// </summary>
    public class ObjectTracker : IChangeTracker
    {
        private OuterObjectsIdentifier _OuterObjectsIdentifier;
        protected ObjectObserver _oObject;
        private ObjectPropertiesTracker _ObjectPropertiesTracker;
        private ObjectTitleTracker _ObjectTitleTracker;
        private ObjectCreation _ObjectCreation;

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
            this.IsTransient = true;
            _OuterObjectsIdentifier = outerObjectsIdentifier;
            _oObject = oObject;
        }

        internal virtual void FinaliseConstruction()
        {
            var tClass = _oObject.Template;

            if (tClass.RealType.IsNonReference())
                return;

            _ObjectPropertiesTracker = new ObjectPropertiesTracker(_oObject);
            _ObjectPropertiesTracker.DetermineInitialState();

            _ObjectTitleTracker = new ObjectTitleTracker(_oObject);
            _ObjectTitleTracker.DetermineInitialState();

            _ObjectCreation = new ObjectCreation()
            {
                Object = _oObject,
                Sequence = SequentialIdGenerator.Next
            };
        }

        /// <summary>
        /// Determines if the associated Object was changed/added/removed. This does NOT include associated dirty objects.
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
        public virtual bool IsTransient { get; set; }

        public bool IsPersistent
        {
            get { return !this.IsTransient; }
        }

        public virtual void DetectChanges()
        {
            //// Skip If this belongs to a property that has NOT been lazy loaded yet:
            //if (_oObject.OuterProperties.All(p => p.IsLazyLoadPending))
            //    return;

            if (_ObjectPropertiesTracker != null &&
                _ObjectPropertiesTracker.DetectChanges())
            {
                _HasLocalChanges = true;
            }

            // TODO: Should this update _HasLocalChanges if the Title has changed?
            _ObjectTitleTracker.DetectChanges();
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

        public IEnumerable<ObjectObserver> PreviousParents
        {
            get { return _oPreviousParents.Values; }
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

        internal virtual void RemoveFromDirtyObjectGraph(ObjectObserver oObject)
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
        /// Resets the dirty flag for this Object
        /// </summary>
        /// <remarks></remarks>
        internal virtual void ResetDirtyFlags()
        {
            //this.IsTransient = false;
            _HasLocalChanges = false;

            _ObjectPropertiesTracker.DetermineInitialState();
            _ObjectTitleTracker.DetermineInitialState();

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

        internal void MarkPropertyChange(BasePropertyObserver oProperty)
        {
            _ObjectPropertiesTracker.DetectChanges(oProperty);
        }

        public ObjectCreation GetObjectCreation()
        {
            return _ObjectCreation;
        }

        public IEnumerable<PropertyChange> GetPropertyChangesSince(long startedAt)
        {
            var results = _ObjectPropertiesTracker.GetChangesSince(startedAt);
            return results;
        }

        public IEnumerable<ObjectTitleChange> GetTitleChangesSince(long startedAt)
        {
            var results = _ObjectTitleTracker.GetChangesSince(startedAt);
            return results;
        }

        public virtual void Dispose()
        {
            _oObject = null;

            _oDirtyObjectGraph.ClearSafely();
            _oDirtyObjectGraph = null;

            _ObjectPropertiesTracker.DisposeSafely();
            _ObjectPropertiesTracker = null;

            _ObjectTitleTracker.DisposeSafely();
            _ObjectTitleTracker = null;
        }

    }
}