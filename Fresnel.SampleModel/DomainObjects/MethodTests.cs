using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.SampleModel
{
    /// <summary>
    /// A set of methods
    /// </summary>
    public class MethodTests : IProgressReporter
    {
        #region IProgressReporter Members

        /// <summary>
        ///
        /// </summary>
        public event ReportProgressEventHandler NotifyProgressUpdate;

        #endregion IProgressReporter Members

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// This method returns no value.
        /// It should appear with a 'ready to invoke' icon.
        /// </summary>
        public void MethodWithNoReturnValue()
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts a single parameter, and will open a dialog.
        /// It should appear with a 'disabled' icon.
        /// </summary>
        /// <param name="dateTime"></param>
        public void MethodWithOneParameter([DataType(DataType.Date)] DateTime dateTime)
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts multiple parameters, and will open a dialog.
        /// It should appear with a 'disabled' icon.
        /// </summary>
        /// <param name="aString">This should accept a String</param>
        /// <param name="aNumber">This should accept an Integer</param>
        /// <param name="aDate">This should accept a Date</param>
        public string MethodWithValueParameters(string aString, int aNumber, DateTime aDate)
        {
            return MethodBase.GetCurrentMethod().Name;
        }

        /// <summary>
        /// This method accepts objects as parameters, and will open a dialog.
        /// </summary>
        /// <param name="category">This should allow ONE Category to be chosen</param>
        /// <param name="pocos">This should allow ONE or MORE PocoObjects to be chosen</param>
        /// <returns></returns>
        public string MethodWithObjectParameters(Category category, IEnumerable<PocoObject> pocos)
        {
            return MethodBase.GetCurrentMethod().Name;
        }

        /// <summary>
        /// This method simply returns a string.
        /// It should appear with a 'ready to invoke' icon.
        /// </summary>
        /// <returns></returns>
        public string MethodThatReturnsA_String()
        {
            return "This is a string";
        }

        /// <summary>
        /// This method returns an Object.
        /// It should appear with a 'ready to invoke' icon.
        /// </summary>
        /// <returns></returns>
        public virtual PocoObject MethodThatReturnsAnObject()
        {
            return new PocoObject();
        }

        /// <summary>
        /// This method uses double-dispatch to call a Factory class
        /// </summary>
        /// <param name="pocoFactory"></param>
        /// <param name="someRandomText"></param>
        /// <returns></returns>
        public virtual PocoObject MethodUsingDoubleDispatch(IFactory<PocoObject> pocoFactory, string someRandomText)
        {
            var result = pocoFactory.Create();
            result.NormalText = someRandomText;
            return result;
        }

        /// <summary>
        /// This method takes 10 seconds to run.
        /// The execution happens on the same thread (the UI is blocked until the method finishes).
        /// </summary>
        /// <returns></returns>
        public string LongRunningSyncMethod()
        {
            var runFor = TimeSpan.FromSeconds(10);
            var runUntil = DateTime.Now.Add(runFor);

            while (DateTime.Now < runUntil)
            {
                System.Threading.Thread.Sleep(1000);
                Trace.TraceInformation("Running...");
            }

            return MethodBase.GetCurrentMethod().Name;
        }

        /// <summary>
        /// This method can be cancelled by the user at any time.
        /// </summary>
        /// <returns></returns>
        public string MethodThatSupportsCancellation()
        {
            var progressArgs = new ProgressEventArgs();
            progressArgs.IsCancellationAllowed = true;

            for (var i = 1; i < 100; i++)
            {
                System.Threading.Thread.Sleep(100);

                if (this.NotifyProgressUpdate != null)
                {
                    progressArgs.PercentComplete = i;
                    progressArgs.Message = string.Concat("Working on step ", i);

                    this.NotifyProgressUpdate(this, progressArgs);

                    if (progressArgs.IsCancellationPending)
                    {
                        return string.Concat("Operation was cancelled after ", i, " steps");
                    }
                }
            }

            return "Done!!";
        }
    }
}