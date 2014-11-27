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
            oNewObject.ChangeTracker.IsNewInstance = true;
        }

        /// <summary>
        /// The given Object was created directly within the given Property
        /// </summary>
        /// <param name="oNewObject"></param>
        /// <param name="oTargetProperty"></param>
        public void ObjectWasCreated(ObjectObserver oNewObject, BasePropertyObserver oTargetProperty)
        {
            oNewObject.ChangeTracker.IsNewInstance = true;

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
            oNewObject.ChangeTracker.IsNewInstance = true;
            oNewObject.ChangeTracker.IsMarkedForAddition = true;

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

            oDirtyObject.ChangeTracker.HasChanges = true;

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

        public void ObjectWasAddedToCollection(ObjectObserver oAddedItem, CollectionObserver oTargetCollection)
        {
            oAddedItem.ChangeTracker.IsMarkedForAddition = true;

            oTargetCollection.ChangeTracker.HasChanges = true;
            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oTargetCollection, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oTargetCollection);
            }
        }

        public void ObjectWasRemovedFromCollection(ObjectObserver oRemovedItem, CollectionObserver oTargetCollection)
        {
            oRemovedItem.ChangeTracker.IsMarkedForRemoval = true;

            oTargetCollection.ChangeTracker.HasChanges = true;
            oRemovedItem.ChangeTracker.MarkForRemovalFrom(oTargetCollection);

            var outerObjects = _OuterObjectsIdentifier.GetOuterObjects(oTargetCollection, int.MaxValue);
            foreach (var outerObject in outerObjects)
            {
                outerObject.ChangeTracker.AddDirtyObject(oTargetCollection);
            }
        }

    }
}
