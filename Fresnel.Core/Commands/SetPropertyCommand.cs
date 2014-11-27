using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class SetPropertyCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private Fresnel.Introspection.Commands.GetPropertyCommand _GetCommand;
        private Fresnel.Introspection.Commands.SetPropertyCommand _SetCommand;

        public SetPropertyCommand
            (
            ObserverCache observerCache,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.GetPropertyCommand getCommand,
            Fresnel.Introspection.Commands.SetPropertyCommand setCommand
            )
        {
            _ObserverCache = observerCache;
            _DirtyObjectNotifier = dirtyObjectNotifier;

            _GetCommand = getCommand;
            _SetCommand = setCommand;
        }

        public void Invoke(BasePropertyObserver oProperty, BaseObjectObserver oValue)
        {
            //var check = this.Permissions.Write.Check();
            //if (check.Failed)
            //{
            //    this.ErrorMessage = check.FailureReason;
            //    return;
            //}
            
            var oOuterObject = oProperty.OuterObject;
            var oObjectProperty = oProperty as ObjectPropertyObserver;
            if (oObjectProperty != null)
            {
                // We're replacing the value, so disassociate the existing value from this property:
                var propertyValue = _GetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name);

                var innerObserver = _ObserverCache.GetObserver(propertyValue);
                if (!innerObserver.IsNullOrEmpty())
                {
                    innerObserver.DisassociateFrom(oObjectProperty);
                }
            }

            _SetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name, oValue.RealObject);

            _DirtyObjectNotifier.PropertyHasChanged(oProperty);

            if (oObjectProperty != null)
            {
                // Make the object aware that it is associated with this property:
                if (!oValue.IsNullOrEmpty())
                {
                    oValue.AssociateWith(oObjectProperty);
                }
            }
        }

    }
}
