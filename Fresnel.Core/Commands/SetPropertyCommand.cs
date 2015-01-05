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
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.GetPropertyCommand _GetCommand;
        private Fresnel.Introspection.Commands.SetPropertyCommand _SetCommand;

        public SetPropertyCommand
            (
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.GetPropertyCommand getCommand,
            Fresnel.Introspection.Commands.SetPropertyCommand setCommand
            )
        {
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
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

            _SetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name, oValue.RealObject);

            _DirtyObjectNotifier.PropertyHasChanged(oProperty);

            // Make sure we know of any changes in the object graph:
            var oObjectProperty = oProperty as ObjectPropertyObserver;
            if (oObjectProperty != null && oObjectProperty.Template.IsAutoProperty)
            {
                // As there's no logic within the property setter, we don't have to scan the whole graph for changes:
                _ObserverCacheSynchroniser.Sync(oObjectProperty, oValue);
            }
            else
            {
                _ObserverCacheSynchroniser.SyncAll();
            }
        }

    }
}
