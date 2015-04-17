using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class EventTimeLine : List<IFresnelEvent>
    {
        ///// <summary>
        ///// Undos the last event
        ///// </summary>
        //public void Undo()
        //{
        //    var lastEvent = this.Last();
        //    lastEvent.Undo();

        //    this.RemoveAt(this.Count - 1);
        //}

        ///// <summary>
        ///// Undoes the last number of events
        ///// </summary>
        ///// <param name="stepsToUndo"></param>
        //public void Undo(int stepsToUndo)
        //{
        //    for (var i = 0; i < stepsToUndo; i++)
        //    {
        //        this.Undo();
        //    }
        //}

        ///// <summary>
        ///// Un-does the events upto the given one
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //public void UndoTo<T>()
        //    where T : IFresnelEvent
        //{
        //    var lastEventOfType = this.LastOrDefault(i => i.GetType() == typeof(T));
        //    var index = this.IndexOf(lastEventOfType);
        //    if (index == -1)
        //        return;

        //    this.Undo(this.Count - 1 - index);
        //}

        //public void UndoAll()
        //{
        //    this.Undo(this.Count);
        //}

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

            var earliestIndex = this.IndexOf(earliestPoint);
            var currentIndex = this.Count - 1;

            while (currentIndex >= earliestIndex)
            {
                var e = this[currentIndex];
                if (e.AffectedObjects.Contains(oObject))
                {
                    e.Undo();
                    this.RemoveAt(currentIndex);
                }

                currentIndex--;
            }
        }
    }

}