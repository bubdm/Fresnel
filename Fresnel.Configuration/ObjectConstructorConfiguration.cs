using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Domain Object constructor
    /// </summary>
    public class ObjectConstructorConfiguration : BaseConfiguration
    {
        public ObjectConstructorConfiguration()
            : base()
        {
            this.CanCreate = true;
        }

        /// <summary>
        /// Determines if the end user can create the object using this constructor.
        /// Useful for preventing Domain Objects with default constructors from being created accidentally.
        /// </summary>
        /// <value></value>
        public bool CanCreate { get; set; }
    }
}