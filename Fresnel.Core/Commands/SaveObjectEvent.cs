using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Envivo.Fresnel.Core.Commands
{
    public class SaveObjectEvent : IFresnelEvent
    {
        public SaveObjectEvent
            (
            ObjectObserver oObject
            )
        {
            this.ObjectToSave = oObject;

            this.AffectedObjects = new ObjectObserver[] { oObject };
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public ObjectObserver ObjectToSave { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }
    }
}