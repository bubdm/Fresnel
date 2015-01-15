using System;


namespace Envivo.Fresnel.Configuration
{

    /// <summary>
    /// Attributes for a class Member
    /// </summary>
    
    [Serializable()]

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class MemberAttribute : BaseAttribute
    {
        public MemberAttribute()
            : base()
        {
            this.Category = "Main";
            this.RequiresConfirmation = true;
        }

        /// <summary>
        /// The category under which this member will appear
        /// </summary>
        /// <value></value>
        public string Category { get; set; }

        /// <summary>
        /// Determines if the user is prompted when changing this member
        /// </summary>
        /// <value></value>
        public bool RequiresConfirmation { get; set; }
    }

}
