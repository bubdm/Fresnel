namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a class Member
    /// </summary>
    public class MemberConfiguration : BaseConfiguration
    {
        public MemberConfiguration()
            : base()
        {
            this.Category = "Main";
            this.RequiresConfirmation = true;
        }

        /// <summary>
        /// The format used to display the member's value/content
        /// </summary>
        public string DisplayFormat { get; set; }

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