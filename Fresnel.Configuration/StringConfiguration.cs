using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a String Property
    /// </summary>
    public class StringConfiguration : PropertyConfiguration
    {
        protected const short DEFAULT_MAX_LENGTH = 80;

        public StringConfiguration()
            : base()
        {
            this.MinLength = 0;
            this.MaxLength = DEFAULT_MAX_LENGTH;
        }

        /// <summary>
        /// The minimum length of this string
        /// </summary>
        /// <value></value>
        public int MinLength { get; set; }

        /// <summary>
        /// The maximum length of this string
        /// </summary>
        /// <value></value>
        public int MaxLength { get; set; }
    }
}