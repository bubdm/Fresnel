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
        private Lazy<IPersistenceService> _PersistenceService;
        private TemplateCache _TemplateCache;
        private ObserverRetriever _ObserverRetriever;
        private ConsistencyCheckCommand _ConsistencyCheckCommand;
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private EventTimeLine _EventTimeLine;

        public SaveObjectCommand
        (
            Lazy<IPersistenceService> persistenceService,
            TemplateCache templateCache,
            ObserverRetriever observerRetriever,
            ConsistencyCheckCommand consistencyCheckCommand,
            DirtyObjectNotifier dirtyObjectNotifier,
            EventTimeLine eventTimeLine
        )
        {
            _PersistenceService = persistenceService;
            _TemplateCache = templateCache;
            _ObserverRetriever = observerRetriever;
            _ConsistencyCheckCommand = consistencyCheckCommand;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _EventTimeLine = eventTimeLine;
        }

        public ActionResult<ObjectObserver[]> Invoke(ObjectObserver oObj)
        {
            // TODO: Until we've found a decent pattern for selectively saving entities, we'll just save everything:
            var objectsToPersist = _ObserverRetriever
                                        .GetAllObservers()
                                        .Where(o => o.Template.IsTrackable)
                                        .Where(o => o.ChangeTracker.IsTransient || o.ChangeTracker.IsDirty || o.ChangeTracker.HasDirtyObjectGraph)
                                        .ToArray();

            // Check that all entities are consistent:
            var checkResult = _ConsistencyCheckCommand.Check(objectsToPersist);
            if (checkResult.Failed)
            {
                return ActionResult<ObjectObserver[]>.Fail(null, checkResult.FailureException);
            }

            // Now save:
            var newEntities = objectsToPersist
                                    .Where(o => o.ChangeTracker.IsTransient)
                                    .Select(o => o.RealObject).ToArray();

            var dirtyEntities = objectsToPersist.Select(o => o.RealObject)
                                    .Except(newEntities)
                                    .ToArray();

            var savedItemCount = _PersistenceService.Value.SaveChanges(newEntities, dirtyEntities);

            var saveEvent = new SaveObjectEvent(oObj);
            _EventTimeLine.Add(saveEvent);

            foreach (var savedObj in objectsToPersist)
            {
                _DirtyObjectNotifier.ObjectIsNoLongerDirty(savedObj);
            }

            return ActionResult<ObjectObserver[]>.Pass(objectsToPersist);
        }

    }
}