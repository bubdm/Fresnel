using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Boolean member
    /// </summary>
    public class BooleanConfigurationBuilder : IConfigurationBuilder
    {

        public BaseConfiguration BuildFrom(IEnumerable<Attribute> attributes)
        {
            var result = new BooleanConfiguration();

            var attr = attributes.OfType<DisplayFormatAttribute>().SingleOrDefault();
            if (attr == null)
            {
                return result;
            }

            if (attr.DataFormatString.IsEmpty())
            {
                return result;
            }

            var parts = attr.DataFormatString.Split('|');
            if (parts.Length > 0)
            {
                result.TrueValue = parts.First();
                result.FalseValue = parts.Last();
            }

            return result;
        }

    }
}