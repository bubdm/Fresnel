using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Observers
{
    public class ParameterObserverMapBuilder
    {
        private Func<MethodObserver, ParameterTemplate, ParameterObserver> _ParameterObserverFactory;

        public ParameterObserverMapBuilder
        (
            Func<MethodObserver, ParameterTemplate, ParameterObserver> parameterObserverFactory
        )
        {
            _ParameterObserverFactory = parameterObserverFactory;
        }

        public ParameterObserverMap BuildFor(MethodObserver oMethod)
        {
            var results = new Dictionary<string, ParameterObserver>();

            var tMethod = oMethod.Template;
            foreach (var tParam in tMethod.Parameters.Values)
            {
                var oParam = _ParameterObserverFactory(oMethod, tParam);
                oParam.FinaliseConstruction();
                results.Add(tParam.Name, oParam);
            }

            return new ParameterObserverMap(results, this.CreateSignatureKey(oMethod));
        }

        private string CreateSignatureKey(MethodObserver oMethod)
        {
            var tMethod = oMethod.Template;

            var parameterNames = new string[tMethod.Parameters.Count];
            var i = 0;
            foreach (var tParameter in tMethod.Parameters.Values)
            {
                parameterNames[i] = tParameter.Name;
                i++;
            }

            return string.Join(",", parameterNames);
        }
    }
}