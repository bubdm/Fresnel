using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a method Parameter
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Enum, AllowMultiple = true)]
    public class ParameterConfiguration : BaseConfiguration
    {
    }
}