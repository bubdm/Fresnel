using Envivo.Fresnel.Configuration;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyTemplateMapBuilder
    {
        private PropertyInfoMapBuilder _PropertyInfoMapBuilder;
        private AttributesMapBuilder _AttributesMapBuilder;
        private PropertyTemplateBuilder _PropertyTemplateBuilder;
        private TrackingPropertiesIdentifier _TrackingPropertiesIdentifier;

        public PropertyTemplateMapBuilder
        (
            PropertyInfoMapBuilder propertyInfoMapBuilder,
            AttributesMapBuilder attributesMapBuilder,
            PropertyTemplateBuilder propertyTemplateBuilder,
            TrackingPropertiesIdentifier trackingPropertiesIdentifier
        )
        {
            _PropertyInfoMapBuilder = propertyInfoMapBuilder;
            _AttributesMapBuilder = attributesMapBuilder;
            _PropertyTemplateBuilder = propertyTemplateBuilder;
            _TrackingPropertiesIdentifier = trackingPropertiesIdentifier;
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

            // HACK: This forces these properties to have the correct Visibility/Persistence configuration:
            _TrackingPropertiesIdentifier.DetermineIdProperty(results.Values);
            _TrackingPropertiesIdentifier.DetermineVersionProperty(results.Values);
            _TrackingPropertiesIdentifier.DetermineAuditProperty(results.Values);

            return new PropertyTemplateMap(results);
        }
    }
}