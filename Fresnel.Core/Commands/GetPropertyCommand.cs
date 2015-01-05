using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
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
        private ObserverCache _ObserverCache;
        private ObserverCacheSynchroniser _ObserverCacheSynchroniser;
        private Fresnel.Introspection.Commands.GetPropertyCommand _GetCommand;
        private RealTypeResolver _RealTypeResolver;

        public GetPropertyCommand
            (
            ObserverCache observerCache,
            ObserverCacheSynchroniser observerCacheSynchroniser,
            Fresnel.Introspection.Commands.GetPropertyCommand getCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverCache = observerCache;
            _ObserverCacheSynchroniser = observerCacheSynchroniser;
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

            var value = _GetCommand.Invoke(oOuterObject.RealObject, oProperty.Template.Name);
            if (value == null)
                return null;

            var valueType = _RealTypeResolver.GetRealType(value);
            var oValue = _ObserverCache.GetObserver(value, valueType);

            var oObjectProperty = oProperty as ObjectPropertyObserver;
            if (oObjectProperty != null)
            {
                var oValueObject = (ObjectObserver)oValue;
                oObjectProperty.IsLazyLoaded = true;

                // Make the object aware that it is associated with this property:
                _ObserverCacheSynchroniser.Sync(oObjectProperty, oValue);
            }

            return oValue;
        }

    }
}
