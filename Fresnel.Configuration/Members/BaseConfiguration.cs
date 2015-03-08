namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// The base class to build concrete Configuration classes
    /// </summary>
    public abstract class BaseConfiguration
    {
        public BaseConfiguration()
        {
            this.IsVisible = true;
            this.Name = string.Empty;
        }

        /// <summary>
        /// Determines if the associated element should be made visible to the user
        /// </summary>
        /// <value></value>
        public bool IsVisible { get; set; }

        /// <summary>
        /// The alternative name to display to the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines if the Attribute values were provided by the consumer (i.e. we're NOT using default values)
        /// </summary>
        public bool IsConfiguredAtRunTime { get; set; }
    }
}