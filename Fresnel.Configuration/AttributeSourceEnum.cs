using System;
namespace Envivo.Fresnel.Configuration
{

    public enum AttributeSource
    {
        /// <summary>
        /// The Attribute was declared directly within the code
        /// </summary>
        Decoration,

        /// <summary>
        /// The Attribute originated from a Fresnel Configuration class
        /// </summary>
        ConfigurationClass,

        /// <summary>
        /// The Attribute was created at run-time by the framework
        /// </summary>
        RunTime
    }
}