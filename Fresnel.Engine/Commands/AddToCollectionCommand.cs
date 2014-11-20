using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Engine.Commands
{
    public class AddToCollectionCommand
    {

        public BaseObjectObserver Invoke(CollectionObserver oCollection, ObjectObserver oItem)
        {

            //ObjectObserver oAddedItem = oItem;

            //if (this.IsReflectionEnabled)
            //{
            //    var preSnapshot = new CollectionChangeSnapshot(this);

            //    // The Add() method might be a function, in which case we want to get the return value:
            //    var returnValue = tAddMethod.Invoke(oMethodOwner.RealObject, new object[] { oItem.RealObject });
            //    var newObject = returnValue;

            //    var postSnapshot = new CollectionChangeSnapshot(this);
            //    if (postSnapshot.IsUnchangedSince(preSnapshot))
            //    {
            //        // Because we don't have any hooks into the list/collection,
            //        // this is the easiest away to determine that nothing changed:
            //        return null;
            //    }

            //    var elementType = this.Template.InnerClass.RealObjectType;
            //    if ((newObject != null) && returnValue.GetRealType().IsDerivedFrom(elementType))
            //    {
            //        // The returned object can be assigned to the Collection:
            //        oAddedItem = this.Session.GetObjectObserver(newObject);
            //    }
            //    else if ((newObject == null) && oItem.RealObjectType.IsDerivedFrom(elementType))
            //    {
            //        // The given Item can be assigned to the Collection:
            //        oAddedItem = oItem;
            //    }
            //}

            //if (oAddedItem != null)
            //{
            //    // Make the item aware that it is associated with this Collection:
            //    this.AssociateWith(oAddedItem);
            //}

            //if (oAddedItem.IsPersistable)
            //{
            //    oAddedItem.ChangeTracker.IsMarkedForAddition = true;
            //}

            //return oAddedItem;

            throw new NotImplementedException();
        }

    }
}
