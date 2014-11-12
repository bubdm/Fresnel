
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{

    public interface ICollectionChangeEventArgs<T>
    {
        /// <summary>
        /// The underlying action that triggered this event
        /// </summary>
        NotifyCollectionChangedAction Action { get; }

        /// <summary>
        /// The item that is being added/removed from the Collection
        /// </summary>
        T Item { get; }

        /// <summary>
        /// The index of the item being added/removed from the Collection
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Determines if the action should be cancelled
        /// </summary>
        bool IsCancelled { get; set; }
    }

}
