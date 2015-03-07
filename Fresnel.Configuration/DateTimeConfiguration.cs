namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a DateTime Property
    /// </summary>
    public class DateTimeConfiguration : PropertyConfiguration
    {
        public DateTimeConfiguration()
            : base()
        {
            this.CustomFormat = "F";
        }

        /// <summary>
        /// The format to be used to display this value.
        /// You may use single character formats as well.
        /// </summary>
        /// <value></value>
        public string CustomFormat { get; set; }
    }
}