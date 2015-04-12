using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Commands
{
    public class AddToCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.AddToCollectionCommand _AddCommand;
        private RealTypeResolver _RealTypeResolver;

        public AddToCollectionCommand
            (
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.AddToCollectionCommand addCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _AddCommand = addCommand;
            _RealTypeResolver = realTypeResolver;
        }

        public BaseObjectObserver Invoke(CollectionObserver oCollection, ObjectObserver oItemToAdd)
        {
            // TODO: Check permissions

            var result = _AddCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToAdd.Template, oItemToAdd.RealObject);

            BaseObjectObserver oAddedItem;
            if (result == null)
            {
                oAddedItem = oItemToAdd;
            }
            else
            {
                var resultType = _RealTypeResolver.GetRealType(result);
                oAddedItem = _ObserverCache.GetObserver(result, resultType);
            }

            var oResult = oAddedItem as ObjectObserver;
            if (oResult != null)
            {
                _DirtyObjectNotifier.ObjectWasAddedToCollection(oResult, oCollection);

                // Make sure we know of any changes in the object graph:
                _ObserverCacheSynchroniser.SyncAll();
            }

            return oAddedItem;
        }
    }
}