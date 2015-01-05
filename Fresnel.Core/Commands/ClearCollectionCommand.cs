using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Core.ChangeTracking;

namespace Envivo.Fresnel.Core.Commands
{
    public class ClearCollectionCommand
    {
        private GetCollectionItemsCommand _GetCollectionItemsCommand;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public ClearCollectionCommand
            (
            GetCollectionItemsCommand getCollectionItemsCommand,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier
            )
        {
            _GetCollectionItemsCommand = getCollectionItemsCommand;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public void Invoke(CollectionObserver oCollection)
        {
            var oItems = _GetCollectionItemsCommand.Invoke(oCollection);
            oCollection.UnbindItemsFromCollection(oItems);

            var oMethod = oCollection.Methods.TryGetValueOrNull("Clear");
            if (oMethod != null)
            {
                var tMethod = oMethod.Template;
                tMethod.Invoke(oCollection.RealObject, null);

                foreach (var oRemovedItem in oItems)
                {
                    _DirtyObjectNotifier.ObjectWasRemovedFromCollection(oRemovedItem, oCollection);
                }
            }

            // Make sure we know of any changes in the object graph:
            _ObserverCacheSynchroniser.SyncAll();
        }

    }
}
