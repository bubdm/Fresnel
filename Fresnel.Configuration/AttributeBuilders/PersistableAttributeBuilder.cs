using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class PersistableAttributeBuilder : IMissingAttributeBuilder
    {
        public bool CanHandle(Type attributeType)
        {
            return attributeType == typeof(PersistableAttribute);
        }

        public Attribute BuildFrom(Type parentClass, Type templateType, IEnumerable<Attribute> templateAttributes)
        {
            var key = templateAttributes.OfType<KeyAttribute>().SingleOrDefault();
            var result = new PersistableAttribute()
            {
                IsAllowed = (key != null)
            };

            return result;
        }
    }
}