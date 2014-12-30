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
    public class AddToCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private Fresnel.Introspection.Commands.AddToCollectionCommand _AddCommand;
        private RealTypeResolver _RealTypeResolver;

        public AddToCollectionCommand
            (
            ObserverCache observerCache,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.AddToCollectionCommand addCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverCache = observerCache;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _AddCommand = addCommand;
            _RealTypeResolver = realTypeResolver;
        }

        public BaseObjectObserver Invoke(CollectionObserver oCollection, ObjectObserver oItemToAdd)
        {
            // TODO: Check permissions 

            //var preSnapshot = new CollectionChangeSnapshot(this);

            var result = _AddCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToAdd.Template, oItemToAdd.RealObject);

            //var postSnapshot = new CollectionChangeSnapshot(this);
            //if (postSnapshot.IsUnchangedSince(preSnapshot))
            //{
            //    // Because we don't have any hooks into the list/collection,
            //    // this is the easiest away to determine that nothing changed:
            //    return null;
            //}

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
                // Make the item aware that it is associated with this Collection:
                oResult.AssociateWith(oCollection);

                _DirtyObjectNotifier.ObjectWasAddedToCollection(oResult, oCollection);
            }

            return oAddedItem;
        }

    }
}
