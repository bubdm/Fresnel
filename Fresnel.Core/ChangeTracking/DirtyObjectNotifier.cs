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
    /// Notifies all objects up the tree when modifieds are made to objects & properties
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
            oNewObject.ChangeTracker.IsTransient = true;
        }

        /// <summary>
        /// The given Object was created directly within the given Property
        /// </summary>
        /// <param name="oNewObject"></param>
        /// <param name="oTargetProperty"></param>
        public void ObjectWasCreated(ObjectObserver oNewObject, BasePropertyObserver oTargetProperty)
        {
            oNewObject.ChangeTracker.IsTransient = true;

            var oOuterObject = oTargetProperty.OuterObject;
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
            oNewObject.ChangeTracker.IsTransient = true;
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
            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oObject, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.RemoveFromDirtyObjectGraph(oObject);
            }
        }

        /// <summary>
        /// The given Object was added to the Collection
        /// </summary>
        /// <param name="oAddedItem"></param>
        /// <param name="oTargetCollection"></param>
        public void ObjectWasAddedToCollection(ObjectObserver oAddedItem, CollectionObserver oTargetCollection)
        {
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
