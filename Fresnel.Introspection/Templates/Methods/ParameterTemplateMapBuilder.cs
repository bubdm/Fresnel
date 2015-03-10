using Envivo.Fresnel.Configuration;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class ParameterTemplateMapBuilder
    {
        private ParameterTemplateBuilder _ParameterTemplateBuilder;
        private AttributesMapBuilder _AttributesMapBuilder;

        public ParameterTemplateMapBuilder
        (
            ParameterTemplateBuilder parameterTemplateBuilder,
            AttributesMapBuilder attributesMapBuilder
        )
        {
            _ParameterTemplateBuilder = parameterTemplateBuilder;
            _AttributesMapBuilder = attributesMapBuilder;
        }

        public ParameterTemplateMap BuildFor(MethodTemplate tMethod)
        {
            var results = new Dictionary<string, ParameterTemplate>();

            foreach (var parameter in tMethod.MethodInfo.GetParameters())
            {
                var paramterAttributes = _AttributesMapBuilder.BuildFor(parameter, tMethod.OuterClass.Configuration);
                var tParameter = _ParameterTemplateBuilder.BuildFor(tMethod, parameter, paramterAttributes);
                results.Add(tParameter.Name, tParameter);
            }

            return new ParameterTemplateMap(results);
        }
    }
}