using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Engine.Commands
{
    public class RemoveFromCollectionCommand
    {

        public BaseObjectObserver Invoke(CollectionObserver oCollection, ObjectObserver oItemToRemove)
        {
            //if (this.IsReflectionEnabled)
            //{
            //    var preSnapshot = new CollectionChangeSnapshot(this);

            //    var args = new object[] { oItem.RealObject };
            //    tRemoveMethod.Invoke(oMethodOwner.RealObject, args);

            //    var postSnapshot = new CollectionChangeSnapshot(this);

            //    if (postSnapshot.IsUnchangedSince(preSnapshot))
            //    {
            //        // Because we don't have any hooks into the list/collection,
            //        // this is the easiest away to determine that nothing changed:
            //        //throw new ObjectNotChangedException();
            //        return false;
            //    }
            //}

            //// NB: Unsaved Domain Objects should NOT affect the dirty status:
            //this.UpdatePersistenceForRemovedItem(oItem);

            //// We can only disassociate after the Persistence settings have been modified:
            //this.DisassociateFrom(oItem);

            //return true;

            throw new NotImplementedException();
        }

    }
}
