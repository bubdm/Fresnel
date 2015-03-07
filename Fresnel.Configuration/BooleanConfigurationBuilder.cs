using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Boolean Property
    /// </summary>
    public class BooleanConfigurationBuilder : IConfigurationBuilder<BooleanConfiguration>
    {

        public BooleanConfiguration BuildFrom(Attribute[] attributes)
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