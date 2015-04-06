using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Envivo.Fresnel.Utils
{
    public class ActionResult<T> : ActionResult
    {
        public static ActionResult<T> Pass<T>(T result)
        {
            var actionResult = new ActionResult<T>()
            {
                Passed = true,
                Result = result
            };
            return actionResult;
        }

        public static ActionResult<T> PassWithWarning(T result, WarningException warning)
        {
            return new ActionResult<T>()
            {
                Passed = true,
                Warning = warning
            };
        }

        public static ActionResult<T> Fail(T result, Exception failure)
        {
            if (failure == null)
                throw new ArgumentNullException("failure");

            return new ActionResult<T>()
            {
                Failed = true,
                FailureException = failure
            };
        }

        public static ActionResult<T> FailWithWarning(T result, Exception failure, WarningException warning)
        {
            if (failure == null)
                throw new ArgumentNullException("failure");

            return new ActionResult<T>()
            {
                Failed = true,
                FailureException = failure,
                Warning = warning
            };
        }

        public T Result { get; private set; }

    }
}