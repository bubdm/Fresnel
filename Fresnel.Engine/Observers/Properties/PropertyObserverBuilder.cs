using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;


namespace Envivo.Fresnel.Engine.Observers
{

    public class PropertyObserverBuilder
    {

        public BasePropertyObserver BuildFor(ObjectObserver oParent, PropertyTemplate tProp)
        {
            if (tProp.IsVisible == false)
                return null;

            var tClass = oParent.TemplateAs<ClassTemplate>();

            var attr = tClass.Attributes.Get<ObjectInstanceAttribute>();
            if (attr.HideAllProperties)
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
