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
        private IDomainDependencyResolver _DomainDependencyResolver;
        private IPersistenceService _PersistenceService;
        private Introspection.Commands.CreateObjectCommand _CreateObjectCommand;

        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public CreateObjectCommand
        (
            IDomainDependencyResolver domainDependencyResolver,
            IPersistenceService persistenceService,
            Introspection.Commands.CreateObjectCommand createObjectCommand,

            TemplateCache templateCache,
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier
        )
        {
            _CreateObjectCommand = createObjectCommand;
            _DomainDependencyResolver = domainDependencyResolver;
            _PersistenceService = persistenceService;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public BaseObjectObserver Invoke(Type classType, IEnumerable<object> args)
        {
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(classType);

            // We have 3 strategies for creating the object:
            // 1) Try the Domain Factory (if it exists)
            // 2) Try the PersistenceService
            // 3) Try the class constructors

            var newInstance = this.CreateObjectUsingDomainFactory(tClass, args);
            if (newInstance == null &&
                _PersistenceService.IsTypeRecognised(classType))
            {
                newInstance = _PersistenceService.CreateObject(classType);
            }
            if (newInstance == null)
            {
                newInstance = _CreateObjectCommand.Invoke(tClass, args);
            }

            SetDefaultId(tClass, newInstance);

            var oNewObject = (ObjectObserver)_ObserverCache.GetObserver(newInstance, classType);

            // Make sure the instance can be edited before the user saves it:
            _DirtyObjectNotifier.ObjectWasCreated(oNewObject);

            // Make sure we know of any changes in the object graph:
            _ObserverCacheSynchroniser.SyncAll();

            return oNewObject;
        }

        private object CreateObjectUsingDomainFactory(ClassTemplate tClass, object constructorArg)
        {
            var genericFactory = typeof(IFactory<>);
            var factoryType = genericFactory.MakeGenericType(tClass.RealType);
            var factory = _DomainDependencyResolver.Resolve(factoryType);

            if (factory == null)
                return null;

            var tFactory = (ClassTemplate)_TemplateCache.GetTemplate(factoryType);

            MethodTemplate tCreateMethod = null;

            if (constructorArg == null)
            {
                // Find a zero-arg method that returns an object:
                tCreateMethod  = tFactory.Methods.Values
                                    .SingleOrDefault(m=> m.MethodInfo.GetParameters().Length == 0 && 
                                                         m.MethodInfo.ReturnType.IsDerivedFrom(tClass.RealType));
            }
            else
            {
                // Find a method that accepts the arg and returns an object:
                var ctorType = constructorArg.GetType();
                tCreateMethod  = tFactory.Methods.Values
                                    .SingleOrDefault(m=> m.MethodInfo.GetParameters().Length == 1 && 
                                                         ctorType.IsDerivedFrom(m.MethodInfo.GetParameters()[0].ParameterType) && 
                                                         m.MethodInfo.ReturnType.IsDerivedFrom(tClass.RealType));
            }

            if (tCreateMethod != null)
            {
                var result = tCreateMethod.Invoke(factory, new object[]{constructorArg});
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