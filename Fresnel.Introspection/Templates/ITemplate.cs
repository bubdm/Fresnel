using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Assemblies;

namespace Envivo.Fresnel.Introspection.Templates
{
    public interface ITemplate
    {
        string Name { get; }

        string FullName { get; }

        string FriendlyName { get; }

        AttributesMap Attributes { get; }

        XmlComments XmlComments { get; }

        /// <summary>
        /// The AssemblyReader associated with this Template
        /// </summary>
        AssemblyReader AssemblyReader { get; }

    }
}
