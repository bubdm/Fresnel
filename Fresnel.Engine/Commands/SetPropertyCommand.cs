using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Engine.Commands
{
    public class SetPropertyCommand
    {

        public void Invoke(BasePropertyObserver oProperty, BaseObjectObserver oValue)
        {
            //if (this.IsReflectionEnabled == false)
            //{
            //    _Value = value;
            //    this.OuterObject.ChangeTracker.HasChanges = true;
            //    return;
            //}

            //var check = this.Permissions.Write.Check();
            //if (check.Failed)
            //{
            //    this.ErrorMessage = check.FailureReason;
            //    return;
            //}

            ////-----

            //// We're replacing the value, so disassociate the existing value from this property:
            //if (_oInnerObserver != null)
            //{
            //    _oInnerObserver.DisassociateFrom(this);
            //}

            //this.PropertyTemplate.SetProperty(this.RealObject, value);

            //this.OuterObject.ChangeTracker.HasChanges = true;

            //// Make the object aware that it is associated with this property:
            //if (value != null)
            //{
            //    this.InnerObserver.AssociateWith(this);
            //}

            throw new NotImplementedException();
        }

    }
}
