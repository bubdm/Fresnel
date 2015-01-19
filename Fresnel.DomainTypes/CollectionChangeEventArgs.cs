using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Specialized;

namespace Envivo.Fresnel.DomainTypes
{
    public class CollectionChangeEventArgs<T> : EventArgs, ICollectionChangeEventArgs<T>
    {
        /// <summary>
        /// The underlying action that triggered this event
        /// </summary>
        public NotifyCollectionChangedAction Action { get; internal set; }

        /// <summary>
        /// The item that is being added/removed from the Collection
        /// </summary>
        public T Item { get; internal set; }

        /// <summary>
        /// The index of the item being added/removed from the Collection
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Determines if the action should be cancelled
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}