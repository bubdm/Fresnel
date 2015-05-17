using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class CancelChangesCommand
    {
        private IEnumerable<IUndoCommand> _UndoCommands;
        private EventTimeLine _EventTimeLine;

        public CancelChangesCommand
            (
            IEnumerable<IUndoCommand> undoCommands,
            EventTimeLine eventTimeLine
            )
        {
            _UndoCommands = undoCommands;
            _EventTimeLine = eventTimeLine;
        }

        public ActionResult Invoke(ObjectObserver oObject)
        {
            var earliestPoint = _EventTimeLine.LastOrDefault(e => e.AffectedObjects.Contains(oObject)) ??
                                _EventTimeLine.FirstOrDefault();

            if (earliestPoint != null)
            {
                this.UndoChangesTo(oObject, earliestPoint);
            }

            // TODO: Reset dirty flags

            return ActionResult.Pass;
        }
             
        public ActionResult Invoke(CollectionObserver oCollection)
        {
            var earliestPoint = _EventTimeLine.LastOrDefault(e => e.AffectedObjects.Contains(oCollection)) ??
                                _EventTimeLine.FirstOrDefault();

            if (earliestPoint != null)
            {
                this.UndoChangesTo(oCollection, earliestPoint);
            }

            // TODO: Reset dirty flags

            return ActionResult.Pass;
        }

        public void UndoChangesTo(ObjectObserver oObject, IFresnelEvent earliestPoint)
        {
            if (oObject == null)
            {
                throw new ArgumentNullException("oObject");
            }
            if (earliestPoint == null)
            {
                throw new ArgumentNullException("earliestPoint");
            }

            var earliestIndex = _EventTimeLine.IndexOf(earliestPoint);
            var currentIndex = _EventTimeLine.Count - 1;

            while (currentIndex >= earliestIndex)
            {
                var e = _EventTimeLine[currentIndex];
                if (e.AffectedObjects.Contains(oObject))
                {
                    var undoCommand = _UndoCommands.FirstOrDefault(c => c.CanHandle(e));
                    if (undoCommand == null)
                        continue;

                    undoCommand.Invoke(e);
                    _EventTimeLine.RemoveAt(currentIndex);
                }

                currentIndex--;
            }
        }

    }
}