
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Assemblies;
using Newtonsoft.Json;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class XmlComments
    {
        /// <summary>
        /// The 'Summary' description extracted from the inline code comments
        /// </summary>
        public string Summary { get; internal set; }

        /// <summary>
        /// The 'Remarks' extracted from the inline code comments
        /// </summary>
        public string Remarks { get; internal set; }

        /// <summary>
        /// The description of the member's Value, extracted from the inline code comments
        /// </summary>
        public string Value { get; internal set; }
    }
}
