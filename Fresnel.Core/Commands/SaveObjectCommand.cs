using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Core.Commands
{
    public class SaveObjectCommand
    {
        private IPersistenceService _PersistenceService;

        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public SaveObjectCommand
        (
            IPersistenceService persistenceService,

            TemplateCache templateCache,
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier
        )
        {
            _PersistenceService = persistenceService;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public ObjectObserver[] Invoke(ObjectObserver oObj)
        {
            // TODO: Until we've found a decent pattern for selectively saving entities, we'll just save everything:
            var observersToPersist = _ObserverCache.GetAllObservers()
                                                     .Where(o => o.ChangeTracker.IsDirty || o.ChangeTracker.HasDirtyObjectGraph)
                                                     .ToArray();
            var dirtyEntities = observersToPersist.Select(o => o.RealObject).ToArray();

            var savedItemCount = _PersistenceService.SaveChanges(dirtyEntities);

            foreach (var savedObj in observersToPersist)
            {
                savedObj.ChangeTracker.ResetDirtyFlags();
            }

            return observersToPersist;
        }

    }
}