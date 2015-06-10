using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// Returns Observers for .NET objects & values
    /// </summary>
    public class ObserverRetriever
    {
        private ObserverCache _ObserverCache;
        private IPersistenceService _PersistenceService;

        public ObserverRetriever
        (
            ObserverCache observerCache,
            IPersistenceService persistenceService
        )
        {
            _ObserverCache = observerCache;
            _PersistenceService = persistenceService;
        }

        public BaseObjectObserver GetObserverById(Guid id)
        {
            var result = _ObserverCache.GetObserverById(id);
            return result;
        }

        /// <summary>
        /// Returns an ObjectObserver from the cache for the given Domain object
        /// </summary>
        /// <param name="obj"></param>
        public BaseObjectObserver GetObserver(object obj)
        {
            var result = _ObserverCache.GetObserver(obj);

            this.UpdatePersistentStatus(result as ObjectObserver); 
            
            return result;
        }

        /// <summary>
        /// Returns an Observer from the cache for the given object Type.
        /// </summary>
        /// <param name="obj">The Object to be observed</param>
        /// <param name="objectType">The Type of the Object to be observed</param>
        public BaseObjectObserver GetObserver(object obj, Type objectType)
        {
            var result = _ObserverCache.GetObserver(obj, objectType);

            this.UpdatePersistentStatus(result as ObjectObserver);

            return result;
        }

        public BaseObjectObserver GetValueObserver(string value, Type valueType)
        {
            var result = _ObserverCache.GetValueObserver(value, valueType);
            return result;
        }

        public IEnumerable<ObjectObserver> GetAllObservers()
        {
            return _ObserverCache.GetAllObservers();
        }

        private void UpdatePersistentStatus(ObjectObserver oObject)
        {
            if (oObject == null)
                return;

            if (oObject.ChangeTracker.IsPersistent)
                // Looks like we've already determined the status:
                return;

            if (!_PersistenceService.IsTypeRecognised(oObject.Template.RealType))
                return;

            if (_PersistenceService.IsPersistent(oObject.ID, oObject.RealObject))
            {
                oObject.MarkAsPersistent();
            }
            else
            {
                oObject.MarkAsTransient();
            }
        }
    }
}