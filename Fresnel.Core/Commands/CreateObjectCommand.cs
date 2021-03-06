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
    public class CreateObjectCommand
    {
        private Lazy<IEnumerable<IFactory>> _DomainObjectFactories;
        private Lazy<IPersistenceService> _PersistenceService;
        private Introspection.Commands.CreateObjectCommand _CreateObjectCommand;

        private TemplateCache _TemplateCache;
        private ObserverRetriever _ObserverRetriever;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public CreateObjectCommand
        (
            Lazy<IEnumerable<IFactory>> domainObjectFactories,
            Lazy<IPersistenceService> persistenceService,
            Introspection.Commands.CreateObjectCommand createObjectCommand,

            TemplateCache templateCache,
            ObserverRetriever observerRetriever,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier
        )
        {
            _CreateObjectCommand = createObjectCommand;
            _DomainObjectFactories = domainObjectFactories;
            _PersistenceService = persistenceService;
            _TemplateCache = templateCache;
            _ObserverRetriever = observerRetriever;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public BaseObjectObserver Invoke(Type classType, object constructorArg)
        {
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(classType);

            // We have 3 strategies for creating the object:
            // 1) Try the Domain Factory (if it exists)
            // 2) Try the PersistenceService
            // 3) Try the IoC container
            // 4) Try the class constructors

            var newInstance = this.CreateObjectUsingDomainFactory(tClass, constructorArg);
            if (newInstance == null)
            {
                newInstance = this.CreateObjectUsingPersistenceService(tClass, constructorArg);
            }
            if (newInstance == null)
            {
                newInstance = _CreateObjectCommand.Invoke(tClass, constructorArg);
            }

            SetDefaultId(tClass, newInstance);

            var oNewObject = (ObjectObserver)_ObserverRetriever.GetObserver(newInstance, classType);

            // Make sure the instance can be edited before the user saves it:
            _DirtyObjectNotifier.ObjectWasCreated(oNewObject);

            // Make sure we know of any changes in the object graph:
            _ObserverCacheSynchroniser.SyncAll();

            return oNewObject;
        }

        private object CreateObjectUsingPersistenceService(ClassTemplate tClass, object constructorArg)
        {
            if (!_PersistenceService.Value.IsTypeRecognised(tClass.RealType))
                return null;

            return _PersistenceService.Value.CreateObject(tClass.RealType, constructorArg);
        }

        private object CreateObjectUsingDomainFactory(ClassTemplate tClass, object constructorArg)
        {
            var genericFactory = typeof(IFactory<>);
            var factoryType = genericFactory.MakeGenericType(tClass.RealType);
            var factory = _DomainObjectFactories.Value.SingleOrDefault(f => f.GetType().IsDerivedFrom(factoryType));

            if (factory == null)
                return null;

            var tFactory = (ClassTemplate)_TemplateCache.GetTemplate(factoryType);

            MethodTemplate tCreateMethod = null;

            if (constructorArg == null)
            {
                // Find a zero-arg method that returns an object:
                tCreateMethod = tFactory.Methods.Values
                                    .SingleOrDefault(m => m.MethodInfo.GetParameters().Length == 0 &&
                                                         m.MethodInfo.ReturnType.IsDerivedFrom(tClass.RealType));
            }
            else
            {
                // Find a method that accepts the arg and returns an object:
                var ctorType = constructorArg.GetType();
                tCreateMethod = tFactory.Methods.Values
                                    .SingleOrDefault(m => m.MethodInfo.GetParameters().Length == 1 &&
                                                         ctorType.IsDerivedFrom(m.MethodInfo.GetParameters()[0].ParameterType) &&
                                                         m.MethodInfo.ReturnType.IsDerivedFrom(tClass.RealType));
            }

            if (tCreateMethod != null)
            {
                var result = tCreateMethod.Invoke(factory, new object[] { constructorArg });
                return result;
            }

            return null;
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