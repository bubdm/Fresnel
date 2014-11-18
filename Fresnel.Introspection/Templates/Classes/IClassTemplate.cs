using System;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Assemblies;

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
