using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a class Member
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class MemberAttribute : BaseAttribute
    {
        public MemberAttribute()
            : base()
        {
            this.IsThreadSafe = true;
            this.Category = "Main";
            this.RequiresConfirmation = true;
        }

        /// <summary>
        /// Determines whether access to this member can be executed on a separate thread,
        /// Set this attibute to FALSE if the member accesses a resource that is not thread safe.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsThreadSafe { get; set; }

        /// <summary>
        /// The category under which this member will appear
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Category { get; set; }

        /// <summary>
        /// Determines if the user is prompted when changing this member
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool RequiresConfirmation { get; set; }
    }

}
