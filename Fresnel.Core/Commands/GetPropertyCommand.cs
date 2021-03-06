using System;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Commands
{
    public class GetPropertyCommand
    {
        private ObserverRetriever _ObserverRetriever;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Lazy<IPersistenceService> _PersistenceService;
        private Fresnel.Introspection.Commands.GetPropertyCommand _GetCommand;
        private RealTypeResolver _RealTypeResolver;

        public GetPropertyCommand
            (
            ObserverRetriever observerRetriever,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            Lazy<IPersistenceService> persistenceService,
            Fresnel.Introspection.Commands.GetPropertyCommand getCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverRetriever = observerRetriever;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
            _PersistenceService = persistenceService;
            _GetCommand = getCommand;
            _RealTypeResolver = realTypeResolver;
        }

        public BaseObjectObserver Invoke(BasePropertyObserver oProperty)
        {
            //var check = this.Permissions.Read.Check();
            //if (check.Failed)
            //{
            //    this.ErrorMessage = check.FailureReason;
            //    return null;
            //}

            ////-----

            var oOuterObject = oProperty.OuterObject;
            var oObjectProperty = oProperty as ObjectPropertyObserver;

            if (oObjectProperty != null)
            {
                if (oOuterObject.ChangeTracker.IsPersistent &&
                     oObjectProperty.IsLazyLoadPending)
                {
                    _PersistenceService.Value.LoadProperty(oOuterObject.RealObject, oProperty.Template.Name);
                }

                // This allows the actual property value to be accessed:
                oObjectProperty.IsLazyLoaded = true;
            }

            var value = _GetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name);

            if (value == null)
            {
                return _ObserverRetriever.GetObserver(null, oProperty.Template.PropertyType);
            }

            var valueType = _RealTypeResolver.GetRealType(value);
            var oValue = _ObserverRetriever.GetObserver(value, valueType);

            if (oObjectProperty != null)
            {
                // Make the object aware that it is associated with this property:
                var oValueObject = (ObjectObserver)oValue;

                _ObserverCacheSynchroniser.Sync(oObjectProperty, oValue);
            }

            return oValue;
        }
    }
}