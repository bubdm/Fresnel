using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Core.Observers
{

    public class MethodObserverBuilder
    {

        private Func<ObjectObserver, MethodTemplate, MethodObserver> _MethodObserverFactory;

        public MethodObserverBuilder
            (
            Func<ObjectObserver, MethodTemplate, MethodObserver> methodObserverFactory
            )
        {
            _MethodObserverFactory = methodObserverFactory;
        }

        public MethodObserver BuildFor(ObjectObserver oParent, MethodTemplate tMethod)
        {
            var tClass = oParent.Template;

            var attr = tClass.Attributes.Get<ObjectInstanceAttribute>();
            if (attr.HideAllMethods)
                return null;

            if (tMethod.IsVisible == false)
                return null;

            var oMethod = _MethodObserverFactory(oParent, tMethod);

            oMethod.FinaliseConstruction();

            return oMethod;
        }

    }

}
