using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a method Parameter
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Enum, AllowMultiple = true)]
    public class ParameterAttribute : BaseAttribute
    {

    }

}
