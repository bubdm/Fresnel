using System;
using System.Globalization;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a Number Property
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class NumberAttribute : PropertyAttribute
    {
        public NumberAttribute()
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
        /// <remarks></remarks>
        public bool IsCurrency { get; set; }

        /// <summary>
        /// The number of decimal places used when displaying this number
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public short DecimalPlaces { get; set; }

        /// <summary>
        /// The minimum value for the property. If specified, the UI will only allow numbers higher than this value.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MinValue { get; set; }

        /// <summary>
        /// The maximum value for the property. If specified, the UI will only allow numbers lower than this value.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MaxValue { get; set; }

    }

}
