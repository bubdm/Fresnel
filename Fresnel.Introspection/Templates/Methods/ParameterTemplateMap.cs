using System.Collections.Generic;

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
