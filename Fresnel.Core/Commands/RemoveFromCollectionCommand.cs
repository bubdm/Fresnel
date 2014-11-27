using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class RemoveFromCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private Fresnel.Introspection.Commands.RemoveFromCollectionCommand _RemoveCommand;
        private RealTypeResolver _RealTypeResolver;

        public RemoveFromCollectionCommand
            (
            ObserverCache observerCache,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.RemoveFromCollectionCommand removeCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverCache = observerCache;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _RemoveCommand = removeCommand;
            _RealTypeResolver = realTypeResolver;
        }

        public bool Invoke(CollectionObserver oCollection, ObjectObserver oItemToRemove)
        {
            // TODO: Check permissions 

            //var preSnapshot = new CollectionChangeSnapshot(this);

            var passed = _RemoveCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToRemove.Template, oItemToRemove.RealObject);

            //var postSnapshot = new CollectionChangeSnapshot(this);
            //if (postSnapshot.IsUnchangedSince(preSnapshot))
            //{
            //    // Because we don't have any hooks into the list/collection,
            //    // this is the easiest away to determine that nothing changed:
            //    return null;
            //}

            if (passed)
            {
                // Make the item is no longer associated with the Collection:
                oItemToRemove.DisassociateFrom(oCollection);

                _DirtyObjectNotifier.ObjectWasRemovedFromCollection(oItemToRemove, oCollection);
            }

            //    var postSnapshot = new CollectionChangeSnapshot(this);

            //    if (postSnapshot.IsUnchangedSince(preSnapshot))
            //    {
            //        // Because we don't have any hooks into the list/collection,
            //        // this is the easiest away to determine that nothing changed:
            //        //throw new ObjectNotChangedException();
            //        return false;
            //    }

            //// NB: Unsaved Domain Objects should NOT affect the dirty status:
            //this.UpdatePersistenceForRemovedItem(oItem);

            return passed;
        }

    }
}
