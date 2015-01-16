using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public interface IClassTemplate : ITemplate
    {

        /// <summary>
        /// The Type of the Object that this Template represents
        /// </summary>
        /// <value></value>
        Type RealType { get; }

        /// <summary>
        /// Returns a user friendly name for the associated object/value
        /// </summary>
        /// <returns></returns>
        string GetFriendlyName(object value);

    }
}
