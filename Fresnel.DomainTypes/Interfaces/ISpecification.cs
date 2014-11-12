
using System.Collections.Generic;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Encapsulates a test to be made against a Domain Object
    /// </summary>
    public interface ISpecification<T>
        where T: class
    {
        /// <summary>
        /// Returns TRUE if this specification is met by the given Domain Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IAssertion IsSatisfiedBy(T obj);

    }
}
