using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class InvokeMethodCommand
    {

        public void Invoke(MethodObserver oMethod)
        {
            //if (this.Parameters.AreRequired && !this.Parameters.IsComplete)
            //{
            //    throw new ArgumentException("One or more Parameters has not been set for this method");
            //}

            //if (!base.IsReflectionEnabled)
            //{
            //    throw new TrueViewException("Reflection has not been enabled for this Observer");
            //}

            //// Build an array of the method's parameter values:
            //object[] parameters = null;
            //if (this.Parameters.Count > 0)
            //{
            //    parameters = new object[Parameters.Count];
            //    var i = 0;
            //    foreach (var oParameter in Parameters.Values)
            //    {
            //        parameters[i] = oParameter.Value;
            //        i += 1;
            //    }
            //}

            //object result = null;
            //try
            //{
            //    result = this.Template.Invoke(this.OuterObject.RealObject, parameters);
            //}
            //catch (Exception ex)
            //{
            //    // If the Method throws an exception, the framework expects the ErrorMessage to be set:
            //    this.ErrorMessage = this.CreateDescriptiveErrorMessage(ex.Message);
            //}
            //finally
            //{
            //    // Reset the parameters so that the method doesn't accidentally get invoked twice in succession:
            //    this.Parameters.Reset();
            //}

            //return result;

            throw new NotImplementedException();
        }

    }
}
