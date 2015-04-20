﻿using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class AddToCollectionEvent : IFresnelEvent
    {
        private AddToCollectionCommand _AddToCollectionCommand;
        private RemoveFromCollectionCommand _RemoveFromCollectionCommand;
        private GetPropertyCommand _GetPropertyCommand;

        private ObjectPropertyObserver _CollectionProperty;
        private CollectionObserver _oCollection;
        private ObjectObserver _oItemToAdd;

        public AddToCollectionEvent
            (
            AddToCollectionCommand addToCollectionCommand,
            RemoveFromCollectionCommand removeFromCollectionCommand,
            GetPropertyCommand getPropertyCommand,
            ObjectPropertyObserver collectionProperty,
            ObjectObserver oItemToAdd
            )
        {
            _AddToCollectionCommand = addToCollectionCommand;
            _RemoveFromCollectionCommand = removeFromCollectionCommand;
            _GetPropertyCommand = getPropertyCommand;

            _CollectionProperty = collectionProperty;
            _oItemToAdd = oItemToAdd;
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

        public ActionResult Do()
        {
            _oCollection = (CollectionObserver)_GetPropertyCommand.Invoke(_CollectionProperty);
            var oAddedItem = (ObjectObserver)_AddToCollectionCommand.Invoke(_oCollection, _oItemToAdd);

            this.AffectedObjects = new ObjectObserver[] { _oCollection, oAddedItem };

            return ActionResult<ObjectObserver>.Pass(oAddedItem);
        }

        public ActionResult Undo()
        {
            var result = _RemoveFromCollectionCommand.Invoke(_oCollection, _oItemToAdd);
            return ActionResult<bool>.Pass(result);
        }
    }
}