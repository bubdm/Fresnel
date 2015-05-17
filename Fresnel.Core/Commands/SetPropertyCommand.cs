using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using System;

namespace Envivo.Fresnel.Core.Commands
{
    public class SetPropertyCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.GetPropertyCommand _GetCommand;
        private Fresnel.Introspection.Commands.SetPropertyCommand _SetCommand;
        private EventTimeLine _EventTimeLine;

        public SetPropertyCommand
            (
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.GetPropertyCommand getCommand,
            Fresnel.Introspection.Commands.SetPropertyCommand setCommand,
            EventTimeLine eventTimeLine
            )
        {
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _DirtyObjectNotifier = dirtyObjectNotifier;

            _GetCommand = getCommand;
            _SetCommand = setCommand;

            _EventTimeLine = eventTimeLine;
        }

        public void Invoke(BasePropertyObserver oProperty, BaseObjectObserver oValue)
        {
            //var check = this.Permissions.Write.Check();
            //if (check.Failed)
            //{
            //    this.ErrorMessage = check.FailureReason;
            //    return;
            //}
            this.PerformValidations(oProperty, oValue);

            var oOuterObject = oProperty.OuterObject;
            var previousValue = _GetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name);

            _SetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name, oValue.RealObject);

            var oObjectProp = oProperty as ObjectPropertyObserver;
            if (oObjectProp != null)
            {
                // This allows the actual property value to be accessed:
                oObjectProp.IsLazyLoaded = true;
            }

            _DirtyObjectNotifier.PropertyHasChanged(oProperty);

            var oPreviousValue = previousValue != null ?
                                    _ObserverCache.GetObserver(previousValue) :
                                    null;
            var setPropertyEvent = new SetPropertyEvent(oProperty, oPreviousValue);
            _EventTimeLine.Add(setPropertyEvent);

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

        private void PerformValidations(BasePropertyObserver oProperty, BaseObjectObserver oValue)
        {
            var attributes = oProperty.Template.Attributes;
            var validations = attributes.RunValidationsFor(oValue.RealObject);
            if (validations.Failed)
                throw validations.FailureException;
        }
    }
}