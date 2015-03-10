using Envivo.Fresnel.Configuration;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class ParameterTemplateMapBuilder
    {
        private ParameterTemplateBuilder _ParameterTemplateBuilder;
        private ConfigurationMapBuilder _ConfigurationMapBuilder;

        public ParameterTemplateMapBuilder
        (
            ParameterTemplateBuilder parameterTemplateBuilder,
            ConfigurationMapBuilder configurationMapBuilder
        )
        {
            _ParameterTemplateBuilder = parameterTemplateBuilder;
            _ConfigurationMapBuilder = configurationMapBuilder;
        }

        public ParameterTemplateMap BuildFor(MethodTemplate tMethod)
        {
            var results = new Dictionary<string, ParameterTemplate>();

            foreach (var parameter in tMethod.MethodInfo.GetParameters())
            {
                var paramterAttributes = _ConfigurationMapBuilder.BuildFor(parameter, tMethod.OuterClass.Configuration);
                var tParameter = _ParameterTemplateBuilder.BuildFor(tMethod, parameter, paramterAttributes);
                results.Add(tParameter.Name, tParameter);
            }

            return new ParameterTemplateMap(results);
        }
    }
}