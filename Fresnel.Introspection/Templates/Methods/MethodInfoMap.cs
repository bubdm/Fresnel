using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class MethodInfoMap : ReadOnlyDictionary<string, MethodInfo>
    {
        public MethodInfoMap(IDictionary<string, MethodInfo> items)
            : base(items)
        {
        }

    }
}