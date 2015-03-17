using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class VisibilityAttributeBuilder : IMissingAttributeBuilder
    {
        public bool CanHandle(Type attributeType)
        {
            return attributeType == typeof(VisibilityAttribute);
        }

        public Attribute BuildFrom(Type parentClass, Type templateType, IEnumerable<Attribute> templateAttributes)
        {
            var result = new VisibilityAttribute()
            {
                IsAllowed = true
            };

            var display = templateAttributes.OfType<DisplayAttribute>().SingleOrDefault();
            if (display == null)
            {
                return result;
            }

            result.IsAllowed = display.GetAutoGenerateField() == null ||
                               display.GetAutoGenerateField().Value;

            return result;
        }
    }
}