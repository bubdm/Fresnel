
using System.Collections.Generic;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// An alternative to boolean values, that provides additional explicit information about an operation
    /// </summary>
    public interface IAssertion
    {
        /// <summary>
        /// Returns TRUE if the Assertion passed, FALSE otherwise
        /// </summary>
        bool Passed { get; }

        /// <summary>
        /// Returns TRUE if the Assertion failed, FALSE otherwise
        /// </summary>
        bool Failed { get; }

        /// <summary>
        /// Returns the reason for the failure (if any)
        /// </summary>
        string FailureReason { get; }

    }
}
