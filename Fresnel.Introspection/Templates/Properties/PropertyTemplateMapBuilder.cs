using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Configuration;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class PropertyTemplateMapBuilder
    {
        private PropertyInfoMapBuilder _PropertyInfoMapBuilder;
        private AttributesMapBuilder _AttributesMapBuilder;
        private PropertyTemplateBuilder _PropertyTemplateBuilder;

        public PropertyTemplateMapBuilder
        (
            PropertyInfoMapBuilder propertyInfoMapBuilder,
            AttributesMapBuilder attributesMapBuilder,
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

            var properties = _PropertyInfoMapBuilder.BuildFor(tClass.RealObjectType);

            foreach (var prop in properties.Values)
            {
                if (prop.GetGetMethod(false) == null)
                    // Ignore properties that cannot be accessed:
                    continue;

                var propertyAttributes = _AttributesMapBuilder.BuildFor(prop, tClass.Configuration);
                var tProp = _PropertyTemplateBuilder.BuildFor(tClass, prop, propertyAttributes);
                results.Add(tProp.Name, tProp);
            }

            return new PropertyTemplateMap(results);
        }

    }

}
