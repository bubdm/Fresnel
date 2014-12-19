using System;


namespace Envivo.Fresnel.Configuration
{

    /// <summary>
    /// Attributes for a Text (i.e. String) Property
    /// </summary>
    
    [Serializable()]

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class TextAttribute : StringAttribute
    {
        public TextAttribute()
            : base()
        {
            this.EditMask = string.Empty;
        }

        /// <summary>
        /// Provides the RegEx mask to use when editing this value in the UI
        /// </summary>
        /// <value></value>
        public string EditMask { get; set; }

    }

}
