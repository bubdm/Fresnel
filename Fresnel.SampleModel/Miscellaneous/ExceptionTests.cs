using Envivo.Fresnel.Configuration;
using System;

namespace Envivo.Fresnel.SampleModel.Miscellaneous
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
        public virtual Guid ID { get; set; }

        /// <summary>
        /// This property will thrown an exception when it is read
        /// </summary>
        public virtual string PropertyWithReadError
        {
            get { throw new ApplicationException("This property threw an exception when it was accessed"); }
            set { _PropertyValue = value; }
        }

        /// <summary>
        /// This property will thrown an exception when it is modified
        /// </summary>
        public virtual string PropertyWithWriteError
        {
            get { return _PropertyValue; }
            set { throw new ApplicationException("This property threw an exception when it was updated"); }
        }

        /// <summary>
        /// This should throw a single exception when the value is set to "Do it!"
        /// </summary>
        [BooleanConfiguration(TrueValue = "Do it!", FalseValue = "Don't do it!")]
        public virtual bool ThrowExceptionIfTrue
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
        [BooleanConfiguration(TrueValue = "Do it!", FalseValue = "Don't do it!")]
        public virtual bool ThrowExceptionIfTrue_Synchronous
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
        public virtual string ThrowAnException()
        {
            throw new ApplicationException("This is a forced exception");
        }
    }
}