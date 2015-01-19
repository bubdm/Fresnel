using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Observers
{
    public class MethodObserverMapBuilder
    {
        private Func<ObjectObserver, MethodTemplate, MethodObserver> _MethodObserverFactory;

        public MethodObserverMapBuilder
        (
            Func<ObjectObserver, MethodTemplate, MethodObserver> methodObserverFactory
        )
        {
            _MethodObserverFactory = methodObserverFactory;
        }

        public MethodObserverMap BuildFor(ObjectObserver oParent)
        {
            var results = new Dictionary<string, MethodObserver>();

            var tClass = oParent.Template;
            foreach (var tMethod in tClass.Methods.Values)
            {
                var oMethod = _MethodObserverFactory(oParent, tMethod);
                oMethod.FinaliseConstruction();

                results.Add(tMethod.Name, oMethod);
            }

            return new MethodObserverMap(results);
        }

        public MethodObserverMap BuildStaticMethodsFor(ObjectObserver oParent)
        {
            var results = new Dictionary<string, MethodObserver>();

            var tClass = oParent.Template;
            foreach (var tMethod in tClass.StaticMethods.Values)
            {
                var oMethod = _MethodObserverFactory(oParent, tMethod);
                oMethod.FinaliseConstruction();

                results.Add(tMethod.Name, oMethod);
            }

            return new MethodObserverMap(results);
        }
    }
}