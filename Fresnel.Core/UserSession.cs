using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Core.Proxies;

namespace Envivo.Fresnel.Core
{
    public class UserSession
    {
        private CreateObjectCommand _CreateObjectCommand;
        private IProxyBuilder _ProxyBuilder;

        public UserSession
            (
            CreateObjectCommand createObjectCommand,
            IProxyBuilder proxyBuilder
            )
        {
            _CreateObjectCommand = createObjectCommand;
            _ProxyBuilder = proxyBuilder;

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
            where T: class
        {
            var oObject = _CreateObjectCommand.Invoke(typeof(T), null);

            var newInstance = (T)oObject.RealObject;

            var proxy = _ProxyBuilder.BuildFor(newInstance);

            return (T)proxy;
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
