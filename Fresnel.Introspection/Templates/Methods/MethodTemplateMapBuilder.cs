using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class MethodTemplateMapBuilder
    {
        private MethodInfoMapBuilder _MethodInfoMapBuilder;
        private AttributesMapBuilder _AttributesMapBuilder;
        private MethodTemplateBuilder _MethodTemplateBuilder;

        public MethodTemplateMapBuilder
        (
            MethodInfoMapBuilder methodInfoMapBuilder,
            AttributesMapBuilder attributesMapBuilder,
            MethodTemplateBuilder methodTemplateBuilder
        )
        {
            _AttributesMapBuilder = attributesMapBuilder;
            _MethodInfoMapBuilder = methodInfoMapBuilder;
            _MethodTemplateBuilder = methodTemplateBuilder;
        }

        public MethodTemplateMap BuildFor(ClassTemplate tClass)
        {
            var methodInfoMap = _MethodInfoMapBuilder.BuildFor(tClass.RealType);
            var result = this.BuildFrom(tClass, methodInfoMap);
            return result;
        }

        public MethodTemplateMap BuildFrom(ClassTemplate tClass, MethodInfoMap methodInfoMap)
        {
            var results = new Dictionary<string, MethodTemplate>();

            foreach (var method in methodInfoMap.Values)
            {
                var methodAttributes = _AttributesMapBuilder.BuildFor(method, tClass.Configuration);
                var tMethod = _MethodTemplateBuilder.BuildFor(tClass, method, methodAttributes);
                results.Add(tMethod.Name, tMethod);
            }

            return new MethodTemplateMap(results, null, null);
        }

        internal void Add(
            MethodTemplate tMethod,
            Dictionary<string, MethodTemplate> existingMethods)
        {
            // Create unique names so that we can deal with overloaded methods:
            if (tMethod.Parameters.Any())
            {
                tMethod.AppendNameWith(this.CreateParametersDescription(tMethod));
            }

            if (existingMethods.Contains(tMethod.Name))
            {
                tMethod.AppendNameWith(this.GetOverloadIndex(tMethod, existingMethods));
            }

            // Add to our 'other' dictionary, so that a consumer can located it by name:
            existingMethods.Add(tMethod.Name, tMethod);
        }

        private string CreateParametersDescription(MethodTemplate tMethod)
        {
            var items = tMethod.Parameters.Values.Select(p => p.ParameterType.Name);
            return string.Concat("(", string.Join(", ", items), ")");
        }

        private string GetOverloadIndex(
            MethodTemplate tMethod,
            Dictionary<string, MethodTemplate> existingMethods)
        {
            var newName = tMethod.Name;
            if (existingMethods.ContainsKey(newName))
            {
                var index = 1;
                var extension = string.Empty;
                while (existingMethods.ContainsKey(newName))
                {
                    extension = "_" + index.ToString();
                    newName = tMethod.Name + extension;
                    index += 1;
                }
                return extension;
            }
            else
            {
                return string.Empty;
            }
        }

    }

}
