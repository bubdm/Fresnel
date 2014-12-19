using System;


namespace Envivo.Fresnel.Configuration
{

    /// <summary>
    /// Attributes for a DateTime Property
    /// </summary>
    
    [Serializable()]
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DateTimeAttribute : PropertyAttribute
    {
        public DateTimeAttribute()
            : base()
        {
            this.CustomFormat = "F";
        }

        /// <summary>
        /// The format to be used to display this value.
        /// You may use single character formats as well.
        /// </summary>
        /// <value></value>
        public string CustomFormat { get; set; }

    }

}
