using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class UndoRemoveFromCollectionCommand : IUndoCommand
    {
        private Introspection.Commands.AddToCollectionCommand _AddToCollectionCommand;

        public UndoRemoveFromCollectionCommand
            (
            Introspection.Commands.AddToCollectionCommand addToCollectionCommand
            )
        {
            _AddToCollectionCommand = addToCollectionCommand;
        }

        public bool CanHandle(IFresnelEvent eventToUndo)
        {
            return eventToUndo is RemoveFromCollectionEvent;
        }

        public ActionResult Invoke(IFresnelEvent eventToUndo)
        {
            return this.Invoke((RemoveFromCollectionEvent)eventToUndo);
        }

        public ActionResult Invoke(RemoveFromCollectionEvent eventToUndo)
        {
            // NB: Do NOT re-use the Core Commands, as they will keep adding new Events to the EventTimeLine
            var oAddedItem = (ObjectObserver)_AddToCollectionCommand.Invoke(eventToUndo.CollectionProperty.OuterObject.RealObject,
                                                                            eventToUndo.CollectionProperty.Template,
                                                                            eventToUndo.RemovedItem.RealObject);
            return ActionResult<ObjectObserver>.Pass(oAddedItem);
        }

    }
}