using System;
using System.ComponentModel;

namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// The base class to build concrete Attribute classes
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    [AttributeUsage(AttributeTargets.All)]
    public abstract class BaseAttribute : System.Attribute
    {
        public BaseAttribute()
        {
            this.IsVisible = true;
            this.Name = string.Empty;
        }

        /// <summary>
        /// Determines if the associated element should be made visible to the user
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsVisible { get; set; }

        /// <summary>
        /// The alternative name to display to the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines if the Attribute values were provided by the consumer (i.e. we're NOT using default values)
        /// </summary>
        internal bool IsConfiguredAtRunTime { get; set; }

    }

}
