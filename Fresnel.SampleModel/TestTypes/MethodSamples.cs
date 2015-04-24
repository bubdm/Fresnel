using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// A set of methods
    /// </summary>
    public class MethodSamples
    {
        private IFactory<Product> _ProductFactory;

        public MethodSamples(IFactory<Product> productFactory)
        {
            _ProductFactory = productFactory;
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// This method returns no value.
        /// It should invoke without showing a dialog.
        /// </summary>
        public void MethodWithNoReturnValue()
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts a single parameter, and will open a dialog.
        /// The method can only be invoked when the user supplies the parameter value.
        /// </summary>
        /// <param name="dateTime"></param>
        public void MethodWithOneParameter([DataType(DataType.Date)] DateTime dateTime)
        {
            Trace.TraceInformation(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// This method accepts multiple parameters, and will open a dialog.
        /// The method can only be invoked when the user supplies the parameter values.
        /// </summary>
        /// <param name="enumFilter">This should be automatically injected</param>
        /// <param name="aString">This should accept a String</param>
        /// <param name="aNumber">This should accept an Integer</param>
        /// <param name="aDate">This should accept a Date</param>
        public string MethodWithValueParameters(IQuerySpecification<EnumValues.IndividualOptions> enumFilter, string aString, int aNumber, DateTime aDate)
        {
            if (enumFilter == null)
            {
                throw new ArgumentNullException("enumFilter");
            }

            var result = string.Concat(MethodBase.GetCurrentMethod().Name,
                                       " executed with the values [",
                                       aString, ", ",
                                       aNumber, ", ",
                                       aDate, "]");
            return result;
        }

        /// <summary>
        /// This method accepts objects as parameters, and will open a dialog.
        /// The method can only be invoked when the user supplies the parameter value.
        /// </summary>
        /// <param name="category">This should allow ONE Category to be chosen</param>
        /// <returns></returns>
        public string MethodWithObjectParameters(Category category)
        {
            return MethodBase.GetCurrentMethod().Name;
        }

        /// <summary>
        /// This method simply returns a string.
        /// It should invoke without showing a dialog, and the result should appear in the UI.
        /// </summary>
        /// <returns></returns>
        public string MethodThatReturnsA_String()
        {
            return "This is a string";
        }

        /// <summary>
        /// This method returns an Object.
        /// It should invoke without showing a dialog, and the result should appear in the UI.
        /// </summary>
        /// <returns></returns>
        public virtual Product MethodThatReturnsAnObject()
        {
            var result = _ProductFactory.Create();
            return result;
        }

        /// <summary>
        /// This method takes 10 seconds to run.
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
    }
}