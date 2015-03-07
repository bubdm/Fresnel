using Envivo.Fresnel.Configuration;
using System;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Number properties
    /// </summary>
    public class NumberValues
    {
        /// <summary>
        ///
        /// </summary>
        public NumberValues()
        {
            this.DoubleNumber = double.NaN;
        }

        private int _IntValue = 0;

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// This is a normal Number
        /// </summary>
        public virtual int NormalNumber
        {
            get { return _IntValue; }
            set { _IntValue = value; }
        }

        /// <summary>
        /// This is a Number with a public Getter, but a hidden Setter.
        /// </summary>
        public virtual int ReadOnlyNumber
        {
            get { return _IntValue; }
            internal set { _IntValue = value; }
        }

        /// <summary>
        /// This is a Number with a hidden Getter, but a public Setter.
        /// This should not be visible in the UI.
        /// </summary>
        public virtual int WriteOnlyNumber
        {
            internal get { return _IntValue; }
            set { _IntValue = value; }
        }

        /// <summary>
        /// This is a public Number, but should be hidden in the UI.
        /// </summary>
        [NumberConfiguration(IsVisible = false)]
        public virtual int HiddenNumber
        {
            get { return _IntValue; }
            set { _IntValue = value; }
        }

        /// <summary>
        /// This is a Number with a range of -234 to +234.
        /// Values beyond the ranges should not be allowed from the UI.
        /// </summary>
        [NumberConfiguration(MinValue = -234, MaxValue = 234)]
        public virtual int NumberWithRange
        {
            get { return _IntValue; }
            set { _IntValue = value; }
        }

        /// <summary>
        /// This is a Float number
        /// </summary>
        public virtual float FloatNumber { get; set; }

        /// <summary>
        /// This is a Double that is shown using CurrentCulture.NumberFormat.CurrencyDecimalDigits
        /// </summary>
        [NumberConfiguration(IsCurrency = true)]
        public virtual double DoubleNumber { get; set; }

        /// <summary>
        /// This is a Float number shown to 5 decimal places
        /// </summary>
        [NumberConfiguration(DecimalPlaces = 5)]
        public virtual float FloatNumberWithPlaces { get; set; }

        /// <summary>
        /// This is a normal Decimal Number
        /// </summary>
        public virtual decimal DecimalNumber { get; set; }

        /// <summary>
        /// This is a number property with a custom title
        /// </summary>
        [PropertyConfiguration(Name = "This name has been made up")]
        internal virtual int CustomNumber
        {
            get { return _IntValue; }
            set { _IntValue = value; }
        }
    }
}