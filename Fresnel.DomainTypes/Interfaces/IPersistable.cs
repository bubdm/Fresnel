using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Any object that may be persisted to a database
    /// </summary>
    public interface IPersistable : IDomainObject
    {
        /// <summary>
        /// The unique identifier for this Entity
        /// </summary>
        Guid ID { get; set; }

        /// <summary>
        /// Used for concurrency checks
        /// </summary>
        long Version { get; set; }
    }
}