using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public interface IClassTemplate : ITemplate
    {

        /// <summary>
        /// The Type of the Object that this Template represents
        /// </summary>
        /// <value></value>

        Type RealObjectType { get; }

    }
}
