using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class MaxLengthAttributeBuilder : IMissingAttributeBuilder
    {
        public bool CanHandle(Type attributeType)
        {
            return attributeType == typeof(MaxLengthAttribute);
        }

        public Attribute BuildFrom(IEnumerable<Attribute> templateAttributes, Type parentClass)
        {
            var result = new MaxLengthAttribute(2000);
            return result;
        }
    }
}