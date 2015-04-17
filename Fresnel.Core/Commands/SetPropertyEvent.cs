using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class SetPropertyEvent : IFresnelEvent
    {
        private SetPropertyCommand _SetPropertyCommand;
        private GetPropertyCommand _GetPropertyCommand;

        private BasePropertyObserver _oProp;
        private BaseObjectObserver _oValue;
        private BaseObjectObserver _oPreviousValue;

        public SetPropertyEvent
            (
            SetPropertyCommand setPropertyCommand,
            GetPropertyCommand getPropertyCommand,
            BasePropertyObserver oProp,
            BaseObjectObserver oItem
            )
        {
            _SetPropertyCommand = setPropertyCommand;
            _GetPropertyCommand = getPropertyCommand;

            _oProp = oProp;
            _oValue = oItem;
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

        public ActionResult Do()
        {
            _oPreviousValue = _GetPropertyCommand.Invoke(_oProp);
            _SetPropertyCommand.Invoke(_oProp, _oValue);

            this.AffectedObjects = new ObjectObserver[] { _oProp.OuterObject };

            return ActionResult.Pass;
        }

        public ActionResult Undo()
        {
            _SetPropertyCommand.Invoke(_oProp, _oPreviousValue);
            return ActionResult.Pass;
        }
    }
}