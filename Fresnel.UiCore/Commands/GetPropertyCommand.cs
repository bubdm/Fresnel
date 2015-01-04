using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;

using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;

using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private Introspection.Commands.GetPropertyCommand _GetPropertyCommand;
        private IClock _Clock;

        public GetPropertyCommand
            (
            Introspection.Commands.GetPropertyCommand getPropertyCommand,
            ObserverCache observerCache,
            AbstractObjectVMBuilder objectVMBuilder,
            IClock clock
        )
        {
            _GetPropertyCommand = getPropertyCommand;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public GetPropertyResult Invoke(GetPropertyRequest request)
        {
            try
            {
                ObjectVM result = null;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;

                if (oObject != null)
                {
                    var oProp = oObject.Properties[request.PropertyName];
                    var oObjectProp = oProp as ObjectPropertyObserver;
                    var returnValue = _GetPropertyCommand.Invoke(oObject.RealObject, request.PropertyName);

                    if (returnValue != null)
                    {
                        var oReturnValue = _ObserverCache.GetObserver(returnValue);

                        if (oObjectProp != null)
                        {
                            oObjectProp.IsLazyLoaded = true;
                            oReturnValue.AssociateWith(oObjectProp);
                        }

                        // Make sure the contents are trackable too:
                        if (oObjectProp.Template.IsCollection)
                        {
                            var oCollection = (CollectionObserver)oReturnValue;
                            var contents = oCollection.GetContents();
                            foreach (var item in contents)
                            {
                                var oItem = _ObserverCache.GetObserver(item, item.GetType());
                                oItem.AssociateWith(oCollection);
                            }
                        }

                        result = _ObjectVMBuilder.BuildFor(oReturnValue);
                    }
                }

                return new GetPropertyResult()
                {
                    Passed = true,
                    ReturnValue = result
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new GetPropertyResult()
                {
                    Failed = true,
                    ErrorMessages = new ErrorVM[] { errorVM }
                };
            }
        }


    }
}
