using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for an Enum Property
    /// </summary>
    public class EnumAttribute : BaseConfiguration
    {
        /// <summary>
        /// The type of Query Specification that is used to restrict the results shown in the UI
        /// </summary>
        public Type ItemFilter { get; set; }
    }
}