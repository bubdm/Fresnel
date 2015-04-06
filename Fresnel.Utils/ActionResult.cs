using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Envivo.Fresnel.Utils
{
    public class ActionResult
    {
        private static ActionResult _Pass = new ActionResult() { Passed = true };

        public static ActionResult Pass
        {
            get { return _Pass; }
        }

        public static ActionResult PassWithWarning(WarningException warning)
        {
            return new ActionResult()
            {
                Passed = true,
                Warning = warning
            };
        }

        public static ActionResult Fail(Exception failure)
        {
            if (failure == null)
                throw new ArgumentNullException("failure");

            return new ActionResult()
            {
                Failed = true,
                FailureException = failure
            };
        }

        public static ActionResult FailWithWarning(Exception failure, WarningException warning)
        {
            if (failure == null)
                throw new ArgumentNullException("failure");

            return new ActionResult()
            {
                Failed = true,
                FailureException = failure,
                Warning = warning
            };
        }

        public bool Passed { get; protected set; }

        public bool Failed { get; protected set; }

        public bool HasWarning { get { return this.Warning != null; } }

        public Exception Warning { get; protected set; }

        public Exception FailureException { get; protected set; }

    }
}