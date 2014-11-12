
using System.Collections.Generic;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Provides access to Factories, Repositories, and other Services within the Domain Model
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Returns the Factory to be used for the given type
        /// </summary>
        /// <typeparam name="T">The Domain Object type created by the factory</typeparam>
        /// <returns>An instance of the Factory</returns>
        IFactory<T> GetFactoryFor<T>()
            where T : class, new();

        /// <summary>
        /// Returns the Factory instance that matches the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An instance of the Factory</returns>
        T GetFactory<T>()
            where T : class, new();

        /// <summary>
        /// Sets the Factory to be used for creating instances of the given type
        /// </summary>
        /// <typeparam name="T">The Domain Object type created by the Factory</typeparam>
        /// <param name="factoryInstance">An instance of the Factory</param>
        void SetFactoryFor<T>(IFactory<T> factoryInstance)
            where T : class, new();


        //-----


        /// <summary>
        /// Returns the Repository to be used for the given type
        /// </summary>
        /// <typeparam name="T">The Domain Object type created by the Repository</typeparam>
        /// <returns>An instance of the Repository</returns>
        IRepository<T> GetRepositoryFor<T>()
            where T : class;

        /// <summary>
        /// Returns the Repository instance that matches the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An instance of the Repository</returns>
        T GetRepository<T>()
            where T : class, new();

        /// <summary>
        /// Sets the Repository to be used for the given type
        /// </summary>
        /// <typeparam name="T">The Aggregate Root type returned by the Repository</typeparam>
        /// <param name="repositoryInstance">An instance of the Repository</param>
        void SetRepositoryFor<T>(IRepository<T> repositoryInstance)
            where T : class;


        //-----


        /// <summary>
        /// Returns the Domain Service implementation matching the given type
        /// </summary>
        /// <typeparam name="T">The type of the required Service</typeparam>
        /// <returns></returns>
        T GetDomainService<T>()
            where T : IDomainService;

        /// <summary>
        /// Sets the given Domain Service implementation
        /// </summary>
        /// <typeparam name="T">The type of the Service</typeparam>
        /// <param name="service">An instance of the Service</param>
        void SetDomainService<T>(T service)
            where T : IDomainService;
    }

}
