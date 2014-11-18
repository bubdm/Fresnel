using System;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Configuration;

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
