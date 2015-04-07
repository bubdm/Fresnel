using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// A set of Exception handling
    /// </summary>
    public class ExceptionTests
    {
        private bool _Bool = false;
        private string _PropertyValue;

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// This property will thrown an exception when it is read
        /// </summary>
        public string PropertyWithReadError
        {
            get { throw new ApplicationException("This property threw an exception when it was accessed"); }
            set { _PropertyValue = value; }
        }

        /// <summary>
        /// This property will thrown an exception when it is modified
        /// </summary>
        public string PropertyWithWriteError
        {
            get { return _PropertyValue; }
            set { throw new ApplicationException("This property threw an exception when it was updated"); }
        }

        /// <summary>
        /// This should throw a single exception when the value is set to "Do it!"
        /// </summary>
        [DisplayFormat(DataFormatString = "Do it!|Don't do it!")]
        public bool ThrowExceptionIfTrue
        {
            get { return _Bool; }
            set
            {
                _Bool = value;
                if (_Bool)
                {
                    throw new ApplicationException("This is a forced exception");
                }
            }
        }

        /// <summary>
        /// This should throw a single exception when the value is set to "Do it!"
        /// </summary>
        [DisplayFormat(DataFormatString = "Do it!|Don't do it!")]
        public bool ThrowExceptionIfTrue_Synchronous
        {
            get { return _Bool; }
            set
            {
                _Bool = value;
                if (_Bool)
                {
                    throw new ApplicationException("This is a forced exception");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string ThrowAnException()
        {
            throw new ApplicationException("This is a forced exception");
        }
    }
}