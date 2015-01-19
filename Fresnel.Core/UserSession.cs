using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.Core
{
    public class UserSession
    {
        private CreateObjectCommand _CreateObjectCommand;

        public UserSession
            (
            CreateObjectCommand createObjectCommand
            )
        {
            _CreateObjectCommand = createObjectCommand;

            this.IdentityMap = new IdentityMap();
            this.UnitOfWork = new UnitOfWork();
        }

        public IdentityMap IdentityMap { get; private set; }

        public UnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Creates an instance of the given Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T New<T>()
            where T : class
        {
            var oObject = _CreateObjectCommand.Invoke(typeof(T), null);

            var newInstance = (T)oObject.RealObject;
            return newInstance;
        }

        /// <summary>
        /// Returns an instance of the object with the given ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T LoadById<T>(Guid id)
        {
            // Defer to a LoadByIdCommand
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Returns the results of the given query
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public IEnumerable<T> LoadQuery(Query query)
        //{
        //// Defer to a LoadQueryCommand
        //   throw new NotImplementedException();
        //}

        public IAssertion Save(params object[] objects)
        {
            // Defer to a SaveCommand
            throw new NotImplementedException();
        }
    }
}