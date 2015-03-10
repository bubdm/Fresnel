namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Boolean Property
    /// </summary>
    public class BooleanConfiguration : BaseConfiguration
    {
        public BooleanConfiguration()
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