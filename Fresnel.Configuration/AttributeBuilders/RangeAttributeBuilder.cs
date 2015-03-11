using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class RangeAttributeBuilder : IMissingAttributeBuilder
    {
        public bool CanHandle(Type attributeType)
        {
            return attributeType == typeof(RangeAttribute);
        }

        public Attribute BuildFrom(IEnumerable<Attribute> templateAttributes, Type parentClass)
        {
            var result = new RangeAttribute(int.MinValue, int.MaxValue);
            return result;
        }
    }
}