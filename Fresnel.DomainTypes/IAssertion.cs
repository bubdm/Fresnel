using System;
using System.Collections.Generic;

namespace Shared.DomainTypes
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

        /// <summary>
        /// Returns the underlying exception
        /// </summary>
        Exception FailureException { get; }

        /// <summary>
        /// Returns an exception created from the underlying failure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>() where T : Exception;

        /// <summary>
        /// Returns TRUE if a warning was raised
        /// </summary>
        bool HasWarning { get; }

        /// <summary>
        /// Returns the warning message (if any)
        /// </summary>
        string WarningReason { get; }

        string GetFailureReason(string delimiter);

        IEnumerable<IAssertion> ToEnumerable();
    }

    public interface IAssertion<T> : IAssertion
    {
        /// <summary>
        /// The value that would be returned naturally by the method
        /// </summary>
        T Result { get; set; }
    }
}
