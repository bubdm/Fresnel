using System;
namespace Envivo.Fresnel.Configuration
{

    public class AttributeEntry
    {
        public AttributeEntry(Attribute attribute, bool isConfiguredAtRunTime)
        {
            this.IsConfiguredAtRunTime = isConfiguredAtRunTime;
            this.Value = attribute;
        }

        /// <summary>
        /// Determines if the Attribute values were provided by the consumer (i.e. we're NOT using default values)
        /// </summary>
        public bool IsConfiguredAtRunTime { get; private set; }

        public Attribute Value { get; private set; }

        public TAttribute ValueAs<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)Value;
        }
    }
}