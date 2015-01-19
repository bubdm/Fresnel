using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;
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
        public void MethodWithOneParameter([DateTime(PreferredInputControl = InputControlTypes.Date)] DateTime dateTime)
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
        /// /// <param name="category">This should accept a Category</param>
        public void MethodWithMultipleParameters(string aString, int aNumber, DateTime aDate, Category category)
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method simply returns a string.
        /// It should appear with a 'ready to invoke' icon.
        /// </summary>
        /// <returns></returns>
        public virtual string MethodThatReturnsA_String()
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
        /// The execution happens on a separate thread.
        /// This button has a custom icon.
        /// </summary>
        [Method(IsAsynchronous = true)]
        public virtual void LongRunningAsyncMethod()
        {
            var runFor = TimeSpan.FromSeconds(10);
            var runUntil = DateTime.Now.Add(runFor);

            while (DateTime.Now < runUntil)
            {
                System.Threading.Thread.Sleep(1000);
                Trace.TraceInformation("Running...");
            }
        }

        /// <summary>
        /// This method takes 10 seconds to run.
        /// The execution happens on the same thread (the UI is blocked until the method finishes).
        /// </summary>
        /// <returns></returns>
        [Method(IsAsynchronous = false)]
        public virtual string LongRunningSyncMethod()
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
        public virtual string MethodThatSupportsCancellation()
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