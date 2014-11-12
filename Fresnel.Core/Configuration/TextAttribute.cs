using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a Text (i.e. String) Property
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class TextAttribute : StringAttribute
    {
        private bool _IsRichText;
        private bool _IsMultiLine;

        public TextAttribute()
            : base()
        {
            this.EditMask = string.Empty;
        }

        /// <summary>
        /// Determines if this value is rendered over multiple lines
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsMultiLine
        {
            get { return _IsMultiLine; }
            set
            {
                _IsMultiLine = value;
                // Make the string a little longer (as long as the default size hasn't changed):
                if (_IsMultiLine && (this.MaxLength == DEFAULT_MAX_LENGTH))
                {
                    this.MaxLength = 255;
                }
            }
        }

        /// <summary>
        /// Determines if this value is rendered as Rich Text.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsRichText
        {
            get { return _IsRichText; }
            set
            {
                _IsRichText = value;
                // Make the string a little longer (as long as the default size hasn't changed):
                if (_IsRichText && (this.MaxLength == DEFAULT_MAX_LENGTH))
                {
                    this.MaxLength = 8000;
                }
            }
        }

        /// <summary>
        /// Determines if this value is hidden in the UI
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsPassword { get; set; }

        /// <summary>
        /// Provides the mask to use when editing this value in the UI
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string EditMask { get; set; }

    }

}
