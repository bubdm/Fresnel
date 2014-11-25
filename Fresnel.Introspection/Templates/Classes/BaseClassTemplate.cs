using Envivo.Fresnel.Configuration;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// The base class for all 'Class' Templates
    /// </summary>
    /// <remarks>This class acts as a wrapper to a .NET class</remarks>
    public abstract class BaseClassTemplate : BaseTemplate, IClassTemplate
    {

        /// <summary>
        /// The custom defined configuration for the associated Domain Class
        /// </summary>
        public IClassConfiguration Configuration { get; internal set; }

        /// <summary>
        /// The Type of the Object that this Template represents
        /// </summary>
        /// <value></value>

        public Type RealType { get; internal set; }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
