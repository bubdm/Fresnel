using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.ComponentModel.DataAnnotations;

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

            var visibility = tClass.Attributes.Get<VisibilityAttribute>();
            if (!visibility.IsAllowed)
                return null;

            if (tMethod.IsVisible == false)
                return null;

            var oMethod = _MethodObserverFactory(oParent, tMethod);

            oMethod.FinaliseConstruction();

            return oMethod;
        }
    }
}