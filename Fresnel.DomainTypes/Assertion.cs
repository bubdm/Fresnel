using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.DomainTypes
{

    /// <summary>
    /// Provides a richer way of checking conditions. Makes a useful replacement for Boolean return types.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class Assertion : IAssertion
    {
        private static readonly Assertion _sPass = new Assertion { Passed = true, Failed = false };

        /// <summary>
        /// Create an Assertion that passed
        /// </summary>
        /// <returns></returns>
        public static Assertion Pass()
        {
            return _sPass;
        }

        /// <summary>
        /// Creates an Assertion that is a Pass, but with a warning
        /// </summary>
        /// <param name="warningReason"></param>
        /// <returns></returns>
        public static Assertion PassWithWarning(string warningReason)
        {
            if (string.IsNullOrEmpty(warningReason))
                throw new ArgumentNullException("warningReason");

            return new Assertion
            {
                Passed = true,
                Failed = false,
                WarningReason = warningReason,
            };
        }

        public static Assertion Fail(string failureReason)
        {
            if (string.IsNullOrEmpty(failureReason))
                throw new ArgumentNullException("failureReason");

            return new Assertion
            {
                Passed = false,
                Failed = true,
                FailureReason = failureReason,
            };
        }

        public static Assertion Fail(string failureReason, Exception exception)
        {
            if (string.IsNullOrEmpty(failureReason))
                throw new ArgumentNullException("failureReason");

            if (exception == null)
                throw new ArgumentNullException("exception");

            return new Assertion
            {
                Passed = false,
                Failed = true,
                FailureReason = failureReason,
                FailureException = exception
            };
        }

        public static Assertion Fail(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            return new Assertion
            {
                Passed = false,
                Failed = true,
                FailureReason = exception.Message,
                FailureException = exception
            };
        }

        /// <summary>
        /// Create an Assertion that failed, with an additional warning
        /// </summary>
        /// <param name="failureReason">The reason for failure</param>
        /// <returns></returns>
        public static Assertion FailWithWarning(string failureReason, string warningReason = null)
        {
            if (string.IsNullOrEmpty(failureReason))
                throw new ArgumentNullException("failureReason");

            return new Assertion
            {
                Passed = false,
                Failed = true,
                FailureReason = failureReason,
                WarningReason = warningReason
            };
        }

        /// <summary>
        /// Returns TRUE if the Assertion passed, FALSE otherwise
        /// </summary>
        public bool Passed { get; protected set; }

        /// <summary>
        /// Returns TRUE if the Assertion failed, FALSE otherwise
        /// </summary>
        public bool Failed { get; protected set; }

        /// <summary>
        /// Returns a reason for the pass or fail
        /// </summary>
        public string FailureReason { get; protected set; }

        /// <summary>
        /// Returns the exception associated with this assertion
        /// </summary>
        public Exception FailureException { get; set; }

        /// <summary>
        /// Returns the original Assertion
        /// </summary>
        public IAssertion InnerAssertion { get; set; }

        /// <summary>
        /// Throws an exception created from the underlying failure
        /// </summary>
        public void Throw<T>()
            where T : Exception
        {
            Exception ex = null;

            if (this.FailureException != null)
            {
                ex = (T)Activator.CreateInstance(typeof(T), this.FailureReason, this.FailureException);
            }
            else if (!string.IsNullOrEmpty(this.FailureReason))
            {
                ex = (T)Activator.CreateInstance(typeof(T), this.FailureReason);
            }

            if (ex != null)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns TRUE if a warning was raised
        /// </summary>
        public bool HasWarning { get { return !string.IsNullOrEmpty(WarningReason); } }

        /// <summary>
        /// Returns the warning message (if any)
        /// </summary>
        public string WarningReason { get; protected set; }

        public override string ToString()
        {
            var result = Passed ?
                           "Passed" :
                           string.Concat("Failed : ", FailureReason);

            if (HasWarning)
            {
                result = string.Concat(result, Environment.NewLine, "Warning : ", WarningReason);
            }

            return result;
        }

        public string GetFailureReason(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
                return delimiter;

            var result = this.FailureReason.Replace(Environment.NewLine, delimiter);
            return result;
        }

        public IEnumerable<IAssertion> ToEnumerable()
        {
            if (InnerAssertion != null)
            {
                return InnerAssertion.ToEnumerable();
            }
            else
            {
                var results = new List<IAssertion>() { this };
                return results;
            }
        }
    }

}