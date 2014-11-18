using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class PropertyTemplateMap : ReadOnlyDictionary<string, PropertyTemplate>
    {

        private IDictionary<PropertyInfo, PropertyTemplate> _tProperties;
        private IEnumerable<PropertyTemplate> _ForObjects;

        public PropertyTemplateMap(Dictionary<string, PropertyTemplate> items)
            : base(items)
        {
            _tProperties = items.Values.ToDictionary(tProp => tProp.PropertyInfo);

            _ForObjects = items.Values.Where(tProp => tProp.IsReferenceType).ToArray();
        }

        public IEnumerable<PropertyTemplate> ForObjects
        {
            get { return _ForObjects; }
        }

        public PropertyTemplate TryGetValueOrNull(PropertyInfo propertyInfo)
        {
            return _tProperties.TryGetValueOrNull(propertyInfo);
        }

    }

}
