using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class RemoveFromCollectionEvent : IFresnelEvent
    {
        private AddToCollectionCommand _AddToCollectionCommand;
        private RemoveFromCollectionCommand _RemoveFromCollectionCommand;
        private GetPropertyCommand _GetPropertyCommand;

        private ObjectPropertyObserver _CollectionProperty;
        private CollectionObserver _oCollection;
        private ObjectObserver _oItemToRemove;

        public RemoveFromCollectionEvent
            (
            AddToCollectionCommand addToCollectionCommand,
            RemoveFromCollectionCommand removeFromCollectionCommand,
            GetPropertyCommand getPropertyCommand,
            ObjectPropertyObserver collectionProperty,
            ObjectObserver oItemToRemove
            )
        {
            _AddToCollectionCommand = addToCollectionCommand;
            _RemoveFromCollectionCommand = removeFromCollectionCommand;
            _GetPropertyCommand = getPropertyCommand;

            _CollectionProperty = collectionProperty;
            _oItemToRemove = oItemToRemove;
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

        public ActionResult Do()
        {
            _oCollection = (CollectionObserver)_GetPropertyCommand.Invoke(_CollectionProperty);
            var result = _RemoveFromCollectionCommand.Invoke(_oCollection, _oItemToRemove);

            this.AffectedObjects = new ObjectObserver[] { _oCollection, _oItemToRemove };

            return ActionResult<bool>.Pass(result);
        }

        public ActionResult Undo()
        {
            var oAddedItem = (ObjectObserver)_AddToCollectionCommand.Invoke(_oCollection, _oItemToRemove);
            return ActionResult<ObjectObserver>.Pass(oAddedItem);
        }
    }
}