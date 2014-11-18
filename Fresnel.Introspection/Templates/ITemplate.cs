using System;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Introspection.Assemblies;

namespace Envivo.Fresnel.Introspection.Templates
{
    public interface ITemplate
    {
        string Name { get; }

        string FullName { get; }

        string FriendlyName { get; }

        string Remarks { get; }

        string Summary { get; }
        
        AttributesMap Attributes { get; }

        /// <summary>
        /// The AssemblyReader associated with this Template
        /// </summary>
        AssemblyReader AssemblyReader { get; }

    }
}
