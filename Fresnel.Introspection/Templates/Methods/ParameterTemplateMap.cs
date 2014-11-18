using System.Collections.Generic;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class ParameterTemplateMap : ReadOnlyDictionary<string, ParameterTemplate>
    {
        public ParameterTemplateMap(Dictionary<string, ParameterTemplate> items)
            : base(items)
        {
        }

    }

}
