using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used to load/save Aggregates of the given type to a Persistence Store.
    /// Inherit and extend this interface to provide access to child objects.
    /// Consider implementing IDependencyAware to access other dependencies.
    /// </summary>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class
    {
        /// <summary>
        /// Loads and returns the Aggregate Root matching the given ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>

        TAggregateRoot Load(Guid id);

        /// <summary>
        /// Saves the given Aggregate Root within a transaction, along with changes made to nested objects within the aggregate (cascading).
        /// </summary>
        /// <param name="aggregateRoot"></param>
        void Save(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Saves the given list of Domain Objects within a transaction. Changes made to other objects within the Aggregate will now be persisted.
        /// </summary>
        /// <param name="objects"></param>

        void Save(params IDomainObject[] domainObjects);

        /// <summary>
        /// Deletes the given Aggregate Root
        /// </summary>
        /// <param name="aggregateRoot"></param>
        void Delete(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Locks the Aggregate Root, and prevents other users from changing it's contents
        /// </summary>
        /// <param name="aggregateRoot"></param>
        IAggregateLock Lock(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Unlocks the Aggregate Root, and allows other users to change it's contents
        /// </summary>
        /// <param name="aggregateRoot"></param>
        void Unlock(TAggregateRoot aggregateRoot);
    }
}