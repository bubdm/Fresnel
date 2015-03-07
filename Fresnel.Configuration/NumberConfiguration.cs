using System;
using System.Globalization;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Number Property
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class NumberConfiguration : PropertyConfiguration
    {
        public NumberConfiguration()
            : base()
        {
            this.DecimalPlaces = (short)CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
            this.MinValue = int.MinValue;
            this.MaxValue = int.MaxValue;
        }

        /// <summary>
        /// Determines if the value is rendered using the locale Currency formatting.
        /// </summary>
        /// <value></value>

        public bool IsCurrency { get; set; }

        /// <summary>
        /// The number of decimal places used when displaying this number
        /// </summary>
        /// <value></value>

        public short DecimalPlaces { get; set; }

        /// <summary>
        /// The minimum value for the property. If specified, the UI will only allow numbers higher than this value.
        /// </summary>
        /// <value></value>

        public int MinValue { get; set; }

        /// <summary>
        /// The maximum value for the property. If specified, the UI will only allow numbers lower than this value.
        /// </summary>
        /// <value></value>

        public int MaxValue { get; set; }
    }
}