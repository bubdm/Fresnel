using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    public interface IAggregateLock
    {
        /// <summary>
        /// Used to apply a pessimistic lock to an Aggregate Root
        /// </summary>
        IAggregateRoot AggregateRoot { get; set; }

        /// <summary>
        /// The user that locked the Aggregate
        /// </summary>
        string LockedBy { get; set; }

        /// <summary>
        /// The time when the lock should be released. Using an 'end time' should prevent rogue locks blocking usage indefinitely.
        /// </summary>
        DateTime LockedUntil { get; set; }
    }
}