using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Date properties
    /// </summary>
    public class DateValues
    {
        private DateTime _DateTime = DateTime.Now;

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// This is an unformatted Date.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        public DateTime NormalDate
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This is a date showing a Time format.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        /// <remarks>
        /// This should ideally show a PropertyGrid instead of a DatePicker
        /// </remarks>
        [DataType(DataType.Time)]
        public DateTime TimeFormat
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This is a date showing a Date format.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DateFormat
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This is a Date showing a custom format "yyyy MMMM dd (dddd) h:mm tt"
        /// </summary>
        [DisplayFormat(DataFormatString = "yyyy MMMM dd (dddd) h:mm tt")]
        public DateTime CustomDateFormat
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This date has no setter, so should be read-only
        /// </summary>
        public DateTime DisabledDateFormat
        {
            get { return _DateTime; }
        }

        private TimeSpan _Timespan;

        /// <summary>
        /// This is a TimeSpan value, and should be editable using an appropriate editor
        /// </summary>
        public TimeSpan Timespan
        {
            get { return _Timespan; }
            set { _Timespan = value; }
        }
    }
}