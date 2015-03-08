using Envivo.Fresnel.Configuration;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyTemplateMapBuilder
    {
        private PropertyInfoMapBuilder _PropertyInfoMapBuilder;
        private ConfigurationMapBuilder _AttributesMapBuilder;
        private PropertyTemplateBuilder _PropertyTemplateBuilder;

        public PropertyTemplateMapBuilder
        (
            PropertyInfoMapBuilder propertyInfoMapBuilder,
            ConfigurationMapBuilder attributesMapBuilder,
            PropertyTemplateBuilder propertyTemplateBuilder
        )
        {
            _PropertyInfoMapBuilder = propertyInfoMapBuilder;
            _AttributesMapBuilder = attributesMapBuilder;
            _PropertyTemplateBuilder = propertyTemplateBuilder;
        }

        public PropertyTemplateMap BuildFor(ClassTemplate tClass)
        {
            var results = new Dictionary<string, PropertyTemplate>();

            var properties = _PropertyInfoMapBuilder.BuildFor(tClass.RealType);

            foreach (var prop in properties.Values)
            {
                if (prop.GetGetMethod(false) == null)
                    // Ignore properties that cannot be accessed:
                    continue;

                var propertyAttributes = _AttributesMapBuilder.BuildFor(prop, tClass.Configuration);
                var tProp = _PropertyTemplateBuilder.BuildFor(tClass, prop, propertyAttributes);
                tProp.AssemblyReader = tClass.AssemblyReader;
                results.Add(tProp.Name, tProp);
            }

            return new PropertyTemplateMap(results);
        }
    }
}