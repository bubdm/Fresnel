using System.ComponentModel;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used to validate Domain Objects
    /// </summary>
    public interface IValidatable : IDataErrorInfo
    {
        /// <summary>
        /// Returns TRUE if the Domain Object is in a valid state. The Error property provides details of actual problems.
        /// </summary>
        bool IsValid();
    }
}