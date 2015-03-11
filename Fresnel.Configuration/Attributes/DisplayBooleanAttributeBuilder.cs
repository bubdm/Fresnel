using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class DisplayBooleanAttributeBuilder : IMissingAttributeBuilder
    {
        public Attribute BuildFrom(IEnumerable<Attribute> templateAttributes, Type parentClass)
        {
            var result = new DisplayBooleanAttribute();

            var displayFormat = templateAttributes.OfType<DisplayFormatAttribute>().SingleOrDefault();
            if (displayFormat == null)
            {
                return result;
            }

            if (displayFormat.DataFormatString.IsEmpty())
            {
                return result;
            }

            var parts = displayFormat.DataFormatString.Split('|');
            if (parts.Length > 0)
            {
                result.TrueName = parts.First();
                result.FalseName = parts.Last();
            }

            return result;
        }
    }
}