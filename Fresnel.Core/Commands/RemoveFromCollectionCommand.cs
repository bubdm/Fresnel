using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class RemoveFromCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private CollectionRemoveMethodIdentifier _CollectionRemoveMethodIdentifier;
        private Introspection.Commands.InvokeMethodCommand _InvokeMethodCommand;
        private Introspection.Commands.RemoveFromCollectionCommand _RemoveCommand;

        public RemoveFromCollectionCommand
            (
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            CollectionRemoveMethodIdentifier collectionRemoveMethodIdentifier,
            Introspection.Commands.InvokeMethodCommand invokeMethodCommand,
            Introspection.Commands.RemoveFromCollectionCommand removeCommand
            )
        {
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _CollectionRemoveMethodIdentifier = collectionRemoveMethodIdentifier;
            _InvokeMethodCommand = invokeMethodCommand;
            _RemoveCommand = removeCommand;
        }

        public bool Invoke(ObjectPropertyObserver oCollectionProp, CollectionObserver oCollection, ObjectObserver oItemToRemove)
        {
            // TODO: Check permissions
            var passed = false;

            // This allows the actual property value to be accessed:
            oCollectionProp.IsLazyLoaded = true;

            var outerAddMethod = this.FindRemoveMethodFromOuterObject(oCollectionProp);
            if (outerAddMethod != null)
            {
                var parentObj = oCollectionProp.OuterObject.RealObject;
                var args = new object[] { oItemToRemove.RealObject };
                _InvokeMethodCommand.Invoke(parentObj, outerAddMethod.Template, args);
                passed = true;
            }
            else
            {
                passed = _RemoveCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToRemove.Template, oItemToRemove.RealObject);
            }

            if (passed)
            {
                _DirtyObjectNotifier.ObjectWasRemovedFromCollection(oItemToRemove, oCollection);

                // Make sure we know of any changes in the object graph:
                _ObserverCacheSynchroniser.SyncAll();
            }

            return passed;
        }

        private MethodObserver FindRemoveMethodFromOuterObject(ObjectPropertyObserver oCollectionProp)
        {
            var tMethod = _CollectionRemoveMethodIdentifier.GetMethod(oCollectionProp.Template);
            if (tMethod == null)
                return null;

            var result = oCollectionProp.OuterObject.Methods.Values.Single(m => m.Template == tMethod);
            return result;
        }

    }
}