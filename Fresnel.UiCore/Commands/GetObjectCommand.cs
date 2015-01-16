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
    public class GetObjectCommand
    {
        private ObserverCache _ObserverCache;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public GetObjectCommand
            (
            ObserverCache observerCache,
            AbstractObjectVMBuilder objectVMBuilder,
            IClock clock
        )
        {
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public GetPropertyResponse Invoke(GetObjectRequest request)
        {
            try
            {
                ObjectVM result = null;

                var oObject = _ObserverCache.GetObserverById(request.ObjectID) as ObjectObserver;
                if (oObject == null)
                    throw new UiCoreException("Cannot find object with ID " + request.ObjectID);

                result = _ObjectVMBuilder.BuildFor(oObject);

                // Done:
                return new GetPropertyResponse()
                {
                    Passed = true,
                    ReturnValue = result
                };
            }
            catch (Exception ex)
            {
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = ex.Message,
                    Detail = ex.ToString(),
                };

                return new GetPropertyResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }


    }
}
