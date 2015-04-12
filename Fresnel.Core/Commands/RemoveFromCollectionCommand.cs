using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Commands
{
    public class RemoveFromCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.RemoveFromCollectionCommand _RemoveCommand;

        public RemoveFromCollectionCommand
            (
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.RemoveFromCollectionCommand removeCommand
            )
        {
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _RemoveCommand = removeCommand;
        }

        public bool Invoke(CollectionObserver oCollection, ObjectObserver oItemToRemove)
        {
            // TODO: Check permissions

            var passed = _RemoveCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToRemove.Template, oItemToRemove.RealObject);

            if (passed)
            {
                _DirtyObjectNotifier.ObjectWasRemovedFromCollection(oItemToRemove, oCollection);

                // Make sure we know of any changes in the object graph:
                _ObserverCacheSynchroniser.SyncAll();
            }

            return passed;
        }
    }
}