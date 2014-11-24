using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Proxies
{

    public class ProxyCache
    {
        private readonly Dictionary<Guid, IFresnelProxy> _ProxyMap = new Dictionary<Guid, IFresnelProxy>();

        private readonly IProxyBuilder _ProxyBuilder;
        private readonly ObserverCache _ObserverCache;
        private CanBeProxiedSpecification _CanBeProxiedSpecification;

        public ProxyCache
            (
            CanBeProxiedSpecification canBeProxiedSpecification,
            ObserverCache observerCache,
            IProxyBuilder proxyBuilder
            )
        {
            _CanBeProxiedSpecification = canBeProxiedSpecification;
            _ObserverCache = observerCache;
            _ProxyBuilder = proxyBuilder;
        }
        /// <summary>
        /// Returns the proxy for the given Domain Object. A new proxy is created if it doesn't exist already.
        /// NB. Repeated calls with the same object will always return the same proxy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        public T GetProxy<T>(T domainObject)
            where T : class
        {
            return this.GetProxy<T>(domainObject, true);
        }

        /// <summary>
        /// Returns the proxy for the given Domain Object. A new proxy is created if it doesn't exist already.
        /// NB. Repeated calls with the same object will always return the same proxy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domainObject"></param>
        /// <param name="throwExceptionForInvalidTypes"></param>
        /// <returns></returns>
        public T GetProxy<T>(T domainObject, bool throwExceptionForInvalidTypes)
            where T : class
        {
            if (domainObject == null)
                throw new ArgumentNullException("domainObject");

            var check = _CanBeProxiedSpecification.IsSatisfiedBy(domainObject);
            if (check.Failed)
            {
                if (throwExceptionForInvalidTypes)
                {
                    throw new ArgumentOutOfRangeException("domainObject", check.FailureReason);
                }
                else
                {
                    //Trace.WriteLine(string.Concat("Cannot create proxy for ", domainObject.ToString(), check.FailureReason));
                    return default(T);
                }
            }

            var oObj = this.GetObserverForProxyUse(domainObject);
            var result = _ProxyMap.TryGetValueOrNull(oObj.ID);
            if (result == null)
            {
                result = _ProxyBuilder.BuildFor(domainObject);
                _ProxyMap.Add(oObj.ID, result);
            }

            return (T)result;
        }

        private BaseObjectObserver GetObserverForProxyUse(object domainObject)
        {
            if (domainObject == null)
            {
                return _ObserverCache.GetObserver(null, typeof(void));
            }
            else
            {
                return _ObserverCache.GetObserver(domainObject, domainObject.GetType());
            }
        }


        /// <summary>
        /// Removes the proxy from the Cache
        /// </summary>
        /// <param name="domainObject"></param>
        public void RemoveProxy(object domainObject)
        {
            var oObj = _ObserverCache.GetObserver(domainObject);
            _ProxyMap.Remove(oObj.ID);
        }

        /// <summary>
        /// Returns the number of ViewModels currently cached
        /// </summary>
        public int Count
        {
            get { return _ProxyMap.Count; }
        }

        internal void Cleanup()
        {
            foreach (var proxy in _ProxyMap.Values)
            {
                var disposable = proxy as IDisposable;
                disposable.Dispose();
            }

            _ProxyMap.Clear();
        }

    }
}
