using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Text(string) properties
    /// </summary>
    public class TextValues
    {
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// This is a normal Char.  It shouldn't allow more than 1 character to be entered.
        /// </summary>
        public virtual char NormalChar { get; set; }

        /// <summary>
        /// This is a normal Text
        /// </summary>
        public virtual string NormalText { get; set; }

        /// <summary>
        /// This is a Text with a public virtual Getter, but a hidden Setter.
        /// </summary>
        public virtual string ReadOnlyText
        {
            get { return this.NormalText; }
            internal set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a Text with a hidden Getter, but a public virtual Setter.
        /// This should not be visible in the UI.
        /// </summary>
        public virtual string WriteOnlyText
        {
            internal get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a public virtual Text, but should be hidden in the UI.
        /// </summary>
        [Text(IsVisible = false)]
        public virtual string HiddenText
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a multi-line Text. Use SHIFT-ENTER to move to the next line.
        /// </summary>
        [Text(PreferredInputControl = InputControlTypes.TextArea)]
        public virtual string MultiLineText
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a Rich Text Text. Use SHIFT-ENTER to move to the next line.
        /// </summary>
        [Text(PreferredInputControl = InputControlTypes.RichTextArea)]
        public virtual string RichTextText
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a password string, and should be shown using asterisks
        /// </summary>
        [Text(PreferredInputControl = InputControlTypes.Password)]
        public virtual string PasswordText
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a Text that cannot exceed 16 characters
        /// </summary>
        [Text(MaxLength = 16)]
        public virtual string TextWithMaximumSize
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This is a Text that must be between 8 and 16 characters in length
        /// </summary>
        [Text(MinLength = 8, MaxLength = 16)]
        internal virtual string TextWithSize
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        /// <summary>
        /// This will force the string to only allow numbers
        /// </summary>
        [Text(MaxLength = 10, EditMask = @"[0-9]*")]
        public virtual string EditMaskText
        {
            get { return this.NormalText; }
            set { this.NormalText = value;}
        }

        private TextValues _Myself;

        /// <summary>
        /// This returns a reference to this object
        /// </summary>
        public virtual TextValues Myself
        {
            get { return this; }
        }

    }
}
