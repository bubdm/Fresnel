using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyInfoMap : ReadOnlyDictionary<string, PropertyInfo>
    {
        public PropertyInfoMap(Dictionary<string, PropertyInfo> items)
            : base(items)
        {
        }
    }
}