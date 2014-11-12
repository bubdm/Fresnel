using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a String Property
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class StringAttribute : PropertyAttribute
    {
        protected const short DEFAULT_MAX_LENGTH = 80;

        public StringAttribute()
            : base()
        {
            this.MinLength = 0;
            this.MaxLength = DEFAULT_MAX_LENGTH;
        }

        /// <summary>
        /// The minimum length of this string
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MinLength { get; set; }

        /// <summary>
        /// The maximum length of this string
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MaxLength { get; set; }

    }

}
