using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Text (i.e. String) Property
    /// </summary>
    public class TextConfiguration : StringConfiguration
    {
        public TextConfiguration()
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