using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class EnumTemplateMap : ReadOnlyDictionary<Type, EnumTemplate>
    {
        public EnumTemplateMap(IDictionary<Type, EnumTemplate> items)
            : base(items)
        {
        }
    }
}