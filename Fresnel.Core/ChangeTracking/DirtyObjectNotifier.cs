using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Notifies all objects up the tree when modifieds are made to objects and properties
    /// </summary>
    public class DirtyObjectNotifier
    {
        private OuterObjectsIdentifier _OuterObjectsIdentifier;

        public DirtyObjectNotifier(OuterObjectsIdentifier outerObjectsIdentifier)
        {
            _OuterObjectsIdentifier = outerObjectsIdentifier;
        }

        /// <summary>
        /// The given Object was newly created
        /// </summary>
        /// <param name="oNewObject"></param>
        public void ObjectWasCreated(ObjectObserver oNewObject)
        {
            oNewObject.MarkAsTransient();
        }

        /// <summary>
        /// The given Object was created directly within the given Property
        /// </summary>
        /// <param name="oNewObject"></param>
        /// <param name="oTargetProperty"></param>
        public void ObjectWasCreated(ObjectObserver oNewObject, ObjectPropertyObserver oTargetProperty)
        {
            oNewObject.MarkAsTransient();
            
            var oOuterObject = oTargetProperty.OuterObject;
            oNewObject.AssociateWith(oTargetProperty);
            oOuterObject.ChangeTracker.AddDirtyObject(oNewObject);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oOuterObject, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oNewObject);
            }
        }

        /// <summary>
        /// The given Object was created directly within the given Collection
        /// </summary>
        /// <param name="oNewObject"></param>
        /// <param name="oTargetCollection"></param>
        public void ObjectWasCreated(ObjectObserver oNewObject, CollectionObserver oTargetCollection)
        {
            oNewObject.MarkAsTransient();
            oNewObject.AssociateWith(oTargetCollection);
            oNewObject.ChangeTracker.MarkForAdditionTo(oTargetCollection);

            oTargetCollection.ChangeTracker.AddDirtyObject(oNewObject);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oTargetCollection, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oNewObject);
            }
        }

        /// <summary>
        /// The given Property was modified
        /// </summary>
        /// <param name="oProperty"></param>
        public void PropertyHasChanged(BasePropertyObserver oProperty)
        {
            var oDirtyObject = oProperty.OuterObject as ObjectObserver;
            if (oDirtyObject == null)
                return;

            oDirtyObject.ChangeTracker.IsDirty = true;
            oDirtyObject.ChangeTracker.MarkPropertyChange(oProperty);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oDirtyObject, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oDirtyObject);
            }
        }

        /// <summary>
        /// The given Object is no longer dirty
        /// </summary>
        /// <param name="oObject"></param>
        public void ObjectIsNoLongerDirty(ObjectObserver oObject)
        {
            // This is to ensure we don't keep bouncing between objects with bi-directional relationships:
            var visitedObjectIds = new Dictionary<Guid, Guid>();
            this.ObjectIsNoLongerDirty(oObject, visitedObjectIds);
        }

        private void ObjectIsNoLongerDirty(ObjectObserver oObject, IDictionary<Guid, Guid> visitiedObjectIds)
        {
            if (visitiedObjectIds.Contains(oObject.ID))
            {
                // We're potentially traversing a bi-directional relationship in the graph.
                // If we've already processed this Observer, there's no need to continue:
                return;
            }

            visitiedObjectIds.Add(oObject.ID, oObject.ID);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oObject, int.MaxValue).ToList();

            oObject.ChangeTracker.ResetDirtyFlags();

            foreach (var outerObject in outerObjects)
            {
                var outerChangeTracker = outerObject.ChangeTracker;
                outerChangeTracker.RemoveFromDirtyObjectGraph(oObject);

                var isOuterObjectNoLongerDirty = !outerChangeTracker.IsDirty && !outerChangeTracker.HasDirtyObjectGraph;
                if (isOuterObjectNoLongerDirty)
                {
                    // Ensure the Outer Object's status is cascaded up the chain:
                    this.ObjectIsNoLongerDirty(outerObject, visitiedObjectIds);
                }
            }
        }



        /// <summary>
        /// The given Object was added to the Collection
        /// </summary>
        /// <param name="oAddedItem"></param>
        /// <param name="oTargetCollection"></param>
        public void ObjectWasAddedToCollection(ObjectObserver oAddedItem, CollectionObserver oTargetCollection)
        {
            oAddedItem.AssociateWith(oTargetCollection);
            oAddedItem.ChangeTracker.MarkForAdditionTo(oTargetCollection);
            oTargetCollection.ChangeTracker.MarkAsAdded(oAddedItem);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oTargetCollection, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oTargetCollection);
            }
        }

        /// <summary>
        /// The given Object was removed from the Collection
        /// </summary>
        /// <param name="oRemovedItem"></param>
        /// <param name="oTargetCollection"></param>
        public void ObjectWasRemovedFromCollection(ObjectObserver oRemovedItem, CollectionObserver oTargetCollection)
        {
            oRemovedItem.DisassociateFrom(oTargetCollection);
            oRemovedItem.ChangeTracker.MarkForRemovalFrom(oTargetCollection);
            oTargetCollection.ChangeTracker.MarkAsRemoved(oRemovedItem);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oTargetCollection, int.MaxValue);
            if (oTargetCollection.ChangeTracker.IsDirty)
            {
                // Let the outer objects know that we've got a dirty collection:
                foreach (var outerObject in outerObjects)
                {
                    outerObject.ChangeTracker.AddDirtyObject(oTargetCollection);
                }
            }
            else
            {
                // Let the outer objects know that the collection isn't dirty:
                foreach (var outerObject in outerObjects)
                {
                    outerObject.ChangeTracker.RemoveFromDirtyObjectGraph(oTargetCollection);
                }
            }
        }
    }
}