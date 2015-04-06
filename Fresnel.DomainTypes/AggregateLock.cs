using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// Used to apply a pessimistic lock to an Aggregate Root
    /// </summary>
    public class AggregateLock : IAggregateLock
    {
        /// <summary>
        /// The Aggregate Root being locked
        /// </summary>
        public IAggregateRoot AggregateRoot { get; set; }

        /// <summary>
        /// The user that locked the Aggregate
        /// </summary>
        public string LockedBy { get; set; }

        /// <summary>
        /// The time when the lock should be released. Using an 'end time' should prevent rogue locks blocking usage indefinitely.
        /// </summary>
        public DateTime LockedUntil { get; set; }
    }
}