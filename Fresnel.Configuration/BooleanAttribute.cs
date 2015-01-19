using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Boolean Property
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class BooleanAttribute : PropertyAttribute
    {
        public BooleanAttribute()
            : base()
        {
            this.TrueValue = "Yes";
            this.FalseValue = "No";
        }

        /// <summary>
        /// The textual value that represents TRUE. The default is "Yes".
        /// </summary>
        /// <value></value>
        /// <remarks>This attribute can be used to provide clarity when rendering boolean values in the UI</remarks>
        public string TrueValue { get; set; }

        /// <summary>
        /// The textual value that represents FALSE. The default is "No".
        /// </summary>
        /// <value></value>
        /// <remarks>This attribute can be used to provide clarity when rendering boolean values in the UI</remarks>
        public string FalseValue { get; set; }
    }
}