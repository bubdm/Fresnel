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
        private SaveObjectCommand _SaveObjectCommand;

        private ObjectObserver _oObject;

        public SaveObjectEvent
            (
            SaveObjectCommand saveObjectCommand,
            ObjectObserver oObject
            )
        {
            _SaveObjectCommand = saveObjectCommand;
            _oObject = oObject;
        }

        public DateTime OccurredAt { get; set; }

        public long SequenceNo { get; set; }

        public IEnumerable<ObjectObserver> AffectedObjects { get; private set; }

        public ActionResult Do()
        {
            var result = _SaveObjectCommand.Invoke(_oObject);

            this.AffectedObjects = new ObjectObserver[] { _oObject };

            return result;
        }

        public ActionResult Undo()
        {
            return ActionResult.PassWithWarning(new WarningException("This action cannot be undone"));
        }
    }
}