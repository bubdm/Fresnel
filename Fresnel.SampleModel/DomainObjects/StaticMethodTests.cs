using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.SampleModel.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.SampleModel
{
    /// <summary>
    /// A set of static methods.
    /// These methods will appear when you right click on the Class.
    /// </summary>
    public class StaticMethodTests
    {
        private StaticMethodTests()
        {
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// This method returns no value.
        /// It should appear with a Green icon.
        /// </summary>
        public static void MethodWithNoReturnValue()
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts a single parameter.
        /// It should appear with a Grey icon.
        /// </summary>
        /// <param name="aString"></param>
        public static void MethodWithOneParameter(string aString)
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts multiple parameters.
        /// It should appear with a Grey icon.
        /// </summary>
        /// <param name="aString">This should accept a String</param>
        /// <param name="aNumber">This should accept an Integer</param>
        /// <param name="aDate">This should accept a Date</param>
        /// /// <param name="category">This should accept a Category</param>
        public static void MethodWithMultipleParameters(string aString, int aNumber, DateTime aDate, Category category)
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method simply returns a string.
        /// It should appear with a Green icon.
        /// </summary>
        /// <returns></returns>
        public static string MethodThatReturnsA_String()
        {
            return "This is a string";
        }

        /// <summary>
        /// This method returns an Entity.
        /// It should appear with a Green icon.
        /// </summary>
        /// <returns></returns>
        public static MethodTests MethodThatReturnsAnEntity()
        {
            return new MethodTests();
        }

        /// <summary>
        /// This method takes 10 seconds to run.
        /// The execution happens on the same thread (the UI is blocked until the method finishes).
        /// </summary>
        /// <returns></returns>
        public static string LongRunningSyncMethod()
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
    }
}