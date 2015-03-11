using System;
namespace Envivo.Fresnel.Configuration
{

    public class AttributeEntry
    {
        /// <summary>
        /// Determines if the Attribute values were provided by the consumer (i.e. we're NOT using default values)
        /// </summary>
        public bool IsConfiguredAtRunTime { get; set; }

        /// <summary>
        /// Returns TRUE if the Attribute was declared in the original code
        /// </summary>
        public bool WasDeclaredInCode { get; set; }

        public Attribute Value { get; set; }

        public TAttribute ValueAs<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)Value;
        }

    }
}