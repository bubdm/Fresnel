
using System.Collections.Generic;
using System.Text;
using System;
using Fresnel.DomainTypes.Interfaces;

namespace Fresnel.DomainTypes
{

    /// <summary>
    /// Provides a richer way of checking conditions. Makes a useful replacement for Boolean return types.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class Assertion : IAssertion
    {

        private static Assertion s_Pass = new Assertion() { Passed = true, Failed = false };

        /// <summary>
        /// Create an Assertion that passed
        /// </summary>
        /// <returns></returns>
        public static Assertion Pass()
        {
            return s_Pass;
        }

        /// <summary>
        /// Create an Assertion that failed
        /// </summary>
        /// <param name="failureReason">The reason for failure</param>
        /// <returns></returns>
        public static Assertion Fail(string failureReason)
        {
            if (string.IsNullOrEmpty(failureReason))
                throw new ArgumentNullException("failureReason");

            return new Assertion()
            {
                Passed = false,
                Failed = true,
                FailureReason = failureReason,
            };
        }

        /// <summary>
        /// Returns TRUE if the Assertion passed, FALSE otherwise
        /// </summary>
        public bool Passed { get; private set; }

        /// <summary>
        /// Returns TRUE if the Assertion failed, FALSE otherwise
        /// </summary>
        public bool Failed { get; private set; }

        /// <summary>
        /// Returns a reason for the pass or fail
        /// </summary>
        public string FailureReason { get; private set; }

        public override string ToString()
        {
            return this.Passed ?
                   "Passed" :
                   string.Concat("Failed : ", this.FailureReason);
        }

    }

}