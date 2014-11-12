
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used to validate Domain Objects
    /// </summary>
    public interface IValidatable : IDataErrorInfo
    {
        /// <summary>
        /// Returns TRUE if the Domain Object is in a valid state. The Error property provides details of actual problems.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsValid();

    }
}
