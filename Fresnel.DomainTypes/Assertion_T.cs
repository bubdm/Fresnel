using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{

    /// <summary>
    /// An Assertion that additionally allows objects/values to be returned to the caller
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class Assertion<T> : Assertion, IAssertion<T>
    {

        public static Assertion<T> Pass(T result)
        {
            return new Assertion<T>()
            {
                 Passed = true,
                 Failed = false,
                 ReturnValue = result
            };
        }

        public static Assertion<T> PassWithWarning(string warningReason, T result)
        {
            if (string.IsNullOrEmpty(warningReason))
                throw new ArgumentNullException("warningReason");

            return new Assertion<T>()
            {
                Passed = true,
                Failed = false,
                WarningReason = warningReason,
                ReturnValue = result
            };
        }

        public static Assertion<T> Fail(string failureReason, T result)
        {
            return new Assertion<T>()
            {
                 Passed = false,
                 Failed = true,
                 FailureReason = failureReason,
                 ReturnValue = result
            };
        }

        public static Assertion<T> Fail(string failureReason, Exception exception, T result)
        {
            return new Assertion<T>()
            {
                Passed = false,
                Failed = true,
                FailureReason = failureReason,
                FailureException = exception,
                ReturnValue = result
            };
        }

        public static Assertion<T> Fail(IAssertion assertion, T result)
        {
            return new Assertion<T>()
            {
                Passed = false,
                Failed = true,
                FailureReason = assertion.FailureReason,
                FailureException = assertion.FailureException,
                InnerAssertion = assertion,
                ReturnValue = result
            };
        }
        /// <summary>
        /// The value that would be returned naturally by the method
        /// </summary>
        public T ReturnValue { get; set; }
    }

}