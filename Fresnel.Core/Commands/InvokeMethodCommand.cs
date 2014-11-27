using Envivo.Fresnel.Core.ChangeTracking;
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
    public class InvokeMethodCommand
    {
        private DirtyObjectNotifier _DirtyObjectNotifier;
        private ObserverCache _ObserverCache;
        private Fresnel.Introspection.Commands.InvokeMethodCommand _InvokeCommand;
        private RealTypeResolver _RealTypeResolver;

        public InvokeMethodCommand
            (
            ObserverCache observerCache,
            DirtyObjectNotifier dirtyObjectNotifier,
            Fresnel.Introspection.Commands.InvokeMethodCommand invokeCommand,
            RealTypeResolver realTypeResolver
            )
        {
            _ObserverCache = observerCache;
            _DirtyObjectNotifier = dirtyObjectNotifier;
            _InvokeCommand = invokeCommand;
            _RealTypeResolver = realTypeResolver;
        }


        public BaseObjectObserver Invoke(MethodObserver oMethod)
        {
            if (oMethod.Parameters.AreRequired &&
                !oMethod.Parameters.IsComplete)
            {
                throw new ArgumentException("One or more Parameters has not been set for this method");
            }

            // TODO: Check permissions

            try
            {
                var args = oMethod.Parameters.Values.Select(p => p.RealObject);

                var oOuterObject = oMethod.OuterObject;

                var result = _InvokeCommand.Invoke(oOuterObject.RealObject, oMethod.Template, args);

                if (result == null)
                    return null;

                var resultType = _RealTypeResolver.GetRealType(result.GetType());
                var oResult = _ObserverCache.GetObserver(result, resultType);

                return oResult;
            }
            finally
            {
                // Reset the parameters so that the method doesn't accidentally get invoked twice in succession:
                oMethod.Parameters.Reset();
            }
        }

    }
}
