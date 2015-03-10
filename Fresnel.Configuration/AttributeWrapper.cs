using System;
namespace Envivo.Fresnel.Configuration
{

    public class AttributeWrapper
    {
        /// <summary>
        /// Determines if the Attribute values were provided by the consumer (i.e. we're NOT using default values)
        /// </summary>
        public bool IsConfiguredAtRunTime { get; internal set; }

        public Attribute Value { get; internal set; }
    }
}