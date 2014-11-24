using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class GetPropertyCommand
    {

        public void Invoke(BasePropertyObserver oProperty)
        {
            //if (this.IsReflectionEnabled == false)
            //{
            //    return _Value;
            //}

            ////-----

            //var check = this.Permissions.Read.Check();
            //if (check.Failed)
            //{
            //    this.ErrorMessage = check.FailureReason;
            //    return null;
            //}

            ////-----

            //try
            //{
            //    return this.PropertyTemplate.GetProperty(this.RealObject);
            //}
            //catch (Exception ex)
            //{
            //    // If the Domain Property throws an exception, the framework expects the ErrorMessage to be set:
            //    this.ErrorMessage = this.CreateDescriptiveErrorMessage(ex.Message);
            //    return null;
            //}

            throw new NotImplementedException();
        }


        /// <summary>
        /// The Observer that proxies this Property's *value*
        /// </summary>
        /// <value>The Observer that proxies this Property's value</value>

        //public override BaseObjectObserver InnerObserver
        //{
        //    get
        //    {
        //        if (this.IsLazyLoadPending)
        //        {
        //            return this.NullObserver;
        //        }

        //        //-----

        //        var readCheck = this.Permissions.Read.Check();
        //        if (readCheck.Failed)
        //        {
        //            this.ErrorMessage = readCheck.FailureReason;
        //            base.InnerObserver = null;
        //            return this.NullObserver;
        //        }

        //        //-----

        //        // NB: It's CRUCIAL that the value is re-fetched from the property 
        //        //     (in case a lazy-load proxy has replaced it):
        //        var value = this.Value;

        //        BaseObjectObserver oParent = null;
        //        if (value != null)
        //        {
        //            oParent = this.Session.GetObserver(value, value.GetRealType());

        //            // CODE SMELL: This bit of code is used to make any lazy-load proxies kick in:
        //            if (oParent.IsPersistable)
        //            {
        //                var dummy = value.GetId().ToString();
        //            }
        //            value = this.Value;

        //            //-----

        //            // Make the object aware that it is associated with this property:
        //            oParent.AssociateWith(this);
        //        }
        //        else
        //        {
        //            oParent = this.Session.ObserverBuilder.CreateNullObserver(this.MemberType);
        //        }

        //        base.InnerObserver = oParent;
        //        return oParent;
        //    }
        //    //TODO: Change InnerObserver to Read-only
        //    set { }
        //}

        
    }
}
