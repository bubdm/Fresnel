using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Introspection.Configuration;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Text(string) properties
    /// </summary>
    public class TextValues
    {
        private string _TextValue = string.Empty;

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as TextValues;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public virtual Guid ID { get; internal set; }

        /// <summary>
        /// This is a normal Char.  It shouldn't allow more than 1 character to be entered.
        /// </summary>
        public virtual char NormalChar { get; set; }

        /// <summary>
        /// This is a normal Text
        /// </summary>
        public virtual string NormalText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a Text with a public virtual Getter, but a hidden Setter.
        /// </summary>
        public virtual string ReadOnlyText
        {
            get { return _TextValue; }
            internal set { _TextValue = value; }
        }

        /// <summary>
        /// This is a Text with a hidden Getter, but a public virtual Setter.
        /// This should not be visible in the UI.
        /// </summary>
        public virtual string WriteOnlyText
        {
            internal get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a public virtual Text, but should be hidden in the UI.
        /// </summary>
        [Text(IsVisible = false)]
        public virtual string HiddenText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a multi-line Text. Use CTRL-ENTER to method new lines.
        /// </summary>
        [Text(IsMultiLine = true)]
        public virtual string MultiLineText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a Rich Text Text. Use CTRL-ENTER to method new lines.
        /// </summary>
        [Text(IsRichText = true)]
        public virtual string RichTextText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a password string, and should be shown using asterisks
        /// </summary>
        [Text(IsPassword = true)]
        public virtual string PasswordText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a Text that cannot exceed 16 characters
        /// </summary>
        [Text(MaxLength = 16)]
        public virtual string TextWithMaximumSize
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This is a Text that must be between 8 and 16 characters in length
        /// </summary>
        [Text(MinLength = 8, MaxLength = 16)]
        internal virtual string TextWithSize
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

        /// <summary>
        /// This will force the string to be shown using a pre-defined format
        /// </summary>
        [Text(EditMask = "(###)##-##-####")]
        public virtual string EditMaskText
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }

    }
}
