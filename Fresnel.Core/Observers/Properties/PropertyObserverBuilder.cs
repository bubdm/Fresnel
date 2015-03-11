using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Observers
{
    public class PropertyObserverBuilder
    {
        public BasePropertyObserver BuildFor(ObjectObserver oParent, PropertyTemplate tProp)
        {
            if (tProp.IsVisible == false)
                return null;

            var tClass = oParent.Template;

            var visibility = tClass.Attributes.Get<VisibilityAttribute>();
            if (!visibility.IsAllowed)
                return null;

            BasePropertyObserver oProperty;
            if (tProp.IsNonReference)
            {
                oProperty = this.CreateNonReferencePropertyObserver(oParent, tProp);
            }
            else
            {
                oProperty = this.CreateObjectPropertyObserver(oParent, tProp);
            }

            oProperty.FinaliseConstruction();

            return oProperty;
        }

        private ObjectPropertyObserver CreateObjectPropertyObserver(ObjectObserver oParent, PropertyTemplate tProperty)
        {
            var oProperty = new ObjectPropertyObserver(oParent, tProperty);
            return oProperty;
        }

        private NonReferencePropertyObserver CreateNonReferencePropertyObserver(ObjectObserver oParent, PropertyTemplate tProperty)
        {
            var oProperty = new NonReferencePropertyObserver(oParent, tProperty);
            return oProperty;
        }
    }
}