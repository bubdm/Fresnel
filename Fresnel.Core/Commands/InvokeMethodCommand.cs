using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Commands
{
    public class InvokeMethodCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverRetriever _ObserverRetriever;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.InvokeMethodCommand _InvokeCommand;
        private RealTypeResolver _RealTypeResolver;
        private IPersistenceService _PersistenceService;
        private IEnumerable<IDomainDependency> _DomainDependencies;

        public InvokeMethodCommand
            (
            ObserverRetriever observerRetriever,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.InvokeMethodCommand invokeCommand,
            RealTypeResolver realTypeResolver,
            IPersistenceService persistenceService,
            IEnumerable<IDomainDependency> domainDependencies
            )
        {
            _ObserverRetriever = observerRetriever;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _InvokeCommand = invokeCommand;
            _RealTypeResolver = realTypeResolver;
            _PersistenceService = persistenceService;
            _DomainDependencies = domainDependencies;
        }

        public BaseObjectObserver Invoke(MethodObserver oMethod, object targetObject)
        {
            // TODO: Check permissions

            try
            {
                this.InjectDomainDependencies(oMethod);

                if (oMethod.Parameters.AreRequired &&
                    !oMethod.Parameters.IsComplete)
                {
                    throw new ArgumentException("One or more Parameters has not been set for this method");
                }

                var args = oMethod.Parameters.Values.Select(p => p.Value);

                // NB: Always use TargetObject instead of oMethod.OuterObject.RealObject
                //     to ensure proxied members are intercepted:
                var result = _InvokeCommand.Invoke(targetObject, oMethod.Template, args);

                if (result == null)
                    return null;

                var resultType = _RealTypeResolver.GetRealType(result);
                var oResult = _ObserverRetriever.GetObserver(result, resultType);

                return oResult;
            }
            finally
            {
                // Make sure we know of any changes in the object graph:
                _ObserverCacheSynchroniser.SyncAll();

                // Reset the parameters so that the method doesn't accidentally get invoked twice in succession:
                oMethod.Parameters.Reset();
            }
        }

        private void InjectDomainDependencies(MethodObserver oMethod)
        {
            foreach (var oParam in oMethod.Parameters.Values)
            {
                var tParam = oParam.Template;
                if (!tParam.IsDomainDependency)
                    continue;

                var dependency = _DomainDependencies.SingleOrDefault(d => d.GetType().IsDerivedFrom(tParam.ParameterType));
                oParam.Value = dependency;
            }
        }

    }
}