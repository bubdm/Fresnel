using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Envivo.Fresnel.Utils;

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
