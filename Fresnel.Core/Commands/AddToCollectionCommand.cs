using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class AddToCollectionCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private CollectionAddMethodIdentifier _CollectionAddMethodIdentifier;
        private Introspection.Commands.InvokeMethodCommand _InvokeMethodCommand;
        private Introspection.Commands.AddToCollectionCommand _AddCommand;
        private RealTypeResolver _RealTypeResolver;
        private EventTimeLine _EventTimeLine;

        public AddToCollectionCommand
            (
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            CollectionAddMethodIdentifier collectionAddMethodIdentifier,
            Introspection.Commands.InvokeMethodCommand invokeMethodCommand,
            Introspection.Commands.AddToCollectionCommand addCommand,
            RealTypeResolver realTypeResolver,
            EventTimeLine eventTimeLine
            )
        {
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _CollectionAddMethodIdentifier = collectionAddMethodIdentifier;
            _InvokeMethodCommand = invokeMethodCommand;
            _AddCommand = addCommand;
            _RealTypeResolver = realTypeResolver;
            _EventTimeLine = eventTimeLine;
        }

        public BaseObjectObserver Invoke(ObjectPropertyObserver oCollectionProp, CollectionObserver oCollection, ObjectObserver oItemToAdd)
        {
            // TODO: Check permissions
            object result = null;

            // This allows the actual property value to be accessed:
            oCollectionProp.IsLazyLoaded = true;

            var outerAddMethod = this.FindAddMethodFromOuterObject(oCollectionProp);
            if (outerAddMethod != null)
            {
                var parentObj = oCollectionProp.OuterObject.RealObject;
                var args = new object[] { oItemToAdd.RealObject };
                result = _InvokeMethodCommand.Invoke(parentObj, outerAddMethod.Template, args);
            }
            else
            {
                result = _AddCommand.Invoke(oCollection.Template, oCollection.RealObject, oItemToAdd.Template, oItemToAdd.RealObject);
            }

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

                var addEvent = new AddToCollectionEvent(oCollectionProp, oCollection, oItemToAdd);
                _EventTimeLine.Add(addEvent);

                // Make sure we know of any changes in the object graph:
                _ObserverCacheSynchroniser.SyncAll();
            }

            return oAddedItem;
        }

        private MethodObserver FindAddMethodFromOuterObject(ObjectPropertyObserver oCollectionProp)
        {
            var tMethod = _CollectionAddMethodIdentifier.GetMethod(oCollectionProp.Template);
            if (tMethod == null)
                return null;

            var result = oCollectionProp.OuterObject.Methods.Values.Single(m => m.Template == tMethod);
            return result;
        }

    }
}