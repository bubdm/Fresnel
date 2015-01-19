using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class CreateObjectCommand
    {
        private Introspection.Commands.CreateObjectCommand _CreateObjectCommand;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public CreateObjectCommand
        (
            Introspection.Commands.CreateObjectCommand createObjectCommand,
            TemplateCache templateCache,
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier
        )
        {
            _CreateObjectCommand = createObjectCommand;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public BaseObjectObserver Invoke(Type classType, IEnumerable<object> args)
        {
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(classType);

            var newInstance = _CreateObjectCommand.Invoke(tClass, args);

            SetDefaultId(tClass, newInstance);

            var oNewObject = (ObjectObserver)_ObserverCache.GetObserver(newInstance);

            // Make sure the instance can be edited before the user saves it:
            _DirtyObjectNotifier.ObjectWasCreated(oNewObject);

            // Make sure we know of any changes in the object graph:
            _ObserverCacheSynchroniser.SyncAll();

            return oNewObject;
        }

        private void SetDefaultId(ClassTemplate tClass, object newInstance)
        {
            if (newInstance == null)
                return;

            var idProperty = tClass.IdProperty;
            if (idProperty == null)
                return;

            var id = (Guid)idProperty.GetProperty(newInstance);
            if (id != Guid.Empty)
                return;

            idProperty.SetField(newInstance, Guid.NewGuid());
        }
    }
}