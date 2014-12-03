using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Date properties
    /// </summary>
    public class DateValues
    {

        private DateTime _DateTime = DateTime.Now;

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
        [DateTime(CustomFormat = "T")]
        public DateTime TimeFormat
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This is a date showing a Date format.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        [DateTime(CustomFormat = "D")]
        public DateTime DateFormat
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        /// <summary>
        /// This is a Date showing a custom format "yyyy MMMM dd (dddd) h:mm tt"
        /// </summary>
        [DateTime(CustomFormat = "yyyy MMMM dd (dddd) h:mm tt")]
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
