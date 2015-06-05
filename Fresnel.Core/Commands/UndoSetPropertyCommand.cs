using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class UndoSetPropertyCommand : IUndoCommand
    {
        private Introspection.Commands.SetPropertyCommand _SetPropertyCommand;

        public UndoSetPropertyCommand
            (
            Introspection.Commands.SetPropertyCommand setPropertyCommand
            )
        {
            _SetPropertyCommand = setPropertyCommand;
        }

        public bool CanHandle(IFresnelEvent eventToUndo)
        {
            return eventToUndo is SetPropertyEvent;
        }

        public ActionResult Invoke(IFresnelEvent eventToUndo)
        {
            return this.Invoke((SetPropertyEvent)eventToUndo);
        }

        public ActionResult Invoke(SetPropertyEvent eventToUndo)
        {
            // NB: Do NOT re-use the Core Commands, as they will keep adding new Events to the EventTimeLine
            _SetPropertyCommand.Invoke(eventToUndo.Property.OuterObject.RealObject,
                                       eventToUndo.Property.Template.Name,
                                       eventToUndo.PreviousValue);
            return ActionResult.Pass;
        }
    }
}