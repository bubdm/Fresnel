using System;
namespace Envivo.Fresnel.Configuration
{

    public class AttributeEntry
    {
        public AttributeSource Source { get; set; }

        public Attribute Value { get; set; }

        public TAttribute ValueAs<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)Value;
        }

    }
}