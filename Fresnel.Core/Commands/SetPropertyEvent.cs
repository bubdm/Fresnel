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
        public SetPropertyEvent
            (
            BasePropertyObserver oProp,
            BaseObjectObserver oPreviousValue
            )
        {
            this.Property = oProp;
            this.PreviousValue = oPreviousValue != null ?
                                    oPreviousValue.RealObject :
                                    null;

            this.AffectedObjects = new ObjectObserver[] { Property.OuterObject };
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public BasePropertyObserver Property { get; private set; }

        public object PreviousValue { get; private set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

    }
}