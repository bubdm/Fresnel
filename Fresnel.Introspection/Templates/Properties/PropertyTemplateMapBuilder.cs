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
        private Func<IClassTemplate, PropertyInfo, AttributesMap, PropertyTemplate> _propertyTemplateFactory;

        public PropertyTemplateMapBuilder
        (
            PropertyInfoMapBuilder propertyInfoMapBuilder,
            AttributesMapBuilder attributesMapBuilder,
            Func<IClassTemplate, PropertyInfo, AttributesMap, PropertyTemplate> propertyTemplateFactory
        )
        {
            _PropertyInfoMapBuilder = propertyInfoMapBuilder;
            _AttributesMapBuilder = attributesMapBuilder;
            _propertyTemplateFactory = propertyTemplateFactory;
        }

        public PropertyTemplateMap BuildFor(BaseClassTemplate tClass)
        {
            var results = new Dictionary<string, PropertyTemplate>();

            var properties = _PropertyInfoMapBuilder.BuildFor(tClass.RealObjectType);

            foreach (var prop in properties.Values)
            {
                if (prop.GetGetMethod(false) == null)
                    // Ignore properties that cannot be accessed:
                    continue;

                var propertyAttributes = _AttributesMapBuilder.BuildFor(prop, tClass.Configuration);
                var tProp = _propertyTemplateFactory(tClass, prop, propertyAttributes);
                results.Add(tProp.Name, tProp);
            }

            return new PropertyTemplateMap(results);
        }

    }

}
