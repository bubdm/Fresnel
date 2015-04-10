using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyTemplateMap : ReadOnlyDictionary<string, PropertyTemplate>
    {
        private IDictionary<PropertyInfo, PropertyTemplate> _tProperties;
        private IEnumerable<PropertyTemplate> _ForObjects;
        private IEnumerable<PropertyTemplate> _VisibleOnly;

        public PropertyTemplateMap(Dictionary<string, PropertyTemplate> items)
            : base(items)
        {
            _tProperties = items.Values.ToDictionary(tp => tp.PropertyInfo);

            _ForObjects = items.Values.Where(tp => tp.IsReferenceType).ToArray();

            _VisibleOnly = items.Values.Where(tp => tp.IsVisible && !tp.IsFrameworkMember).ToArray();
        }

        /// <summary>
        /// Returns the Properties that are for reference values
        /// </summary>
        public IEnumerable<PropertyTemplate> ForObjects
        {
            get { return _ForObjects; }
        }

        /// <summary>
        /// Returns the Properties that are for end user usage
        /// </summary>
        public IEnumerable<PropertyTemplate> VisibleOnly
        {
            get { return _VisibleOnly; }
        }

        public PropertyTemplate TryGetValueOrNull(PropertyInfo propertyInfo)
        {
            return _tProperties.TryGetValueOrNull(propertyInfo);
        }
    }
}