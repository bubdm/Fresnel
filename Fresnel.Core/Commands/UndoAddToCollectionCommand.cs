using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class UndoAddToCollectionCommand : IUndoCommand
    {
        private Introspection.Commands.RemoveFromCollectionCommand _RemoveFromCollectionCommand;

        public UndoAddToCollectionCommand
            (
            Introspection.Commands.RemoveFromCollectionCommand removeFromCollectionCommand
            )
        {
            _RemoveFromCollectionCommand = removeFromCollectionCommand;
        }

        public bool CanHandle(IFresnelEvent eventToUndo)
        {
            return eventToUndo is AddToCollectionEvent;
        }

        public ActionResult Invoke(IFresnelEvent eventToUndo)
        {
            return this.Invoke((AddToCollectionEvent)eventToUndo);
        }

        public ActionResult Invoke(AddToCollectionEvent eventToUndo)
        {
            // NB: Do NOT re-use the Core Commands, as they will keep adding new Events to the EventTimeLine
            var result = _RemoveFromCollectionCommand.Invoke(eventToUndo.CollectionProperty.OuterObject.RealObject, 
                                                             eventToUndo.CollectionProperty.Template, 
                                                            eventToUndo.AddedItem.RealObject);
            return ActionResult<bool>.Pass(result);
        }
    }
}