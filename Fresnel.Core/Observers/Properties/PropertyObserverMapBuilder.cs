using Envivo.Fresnel.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Observers
{
    public class PropertyObserverMapBuilder
    {
        private PropertyObserverBuilder _PropertyObserverBuilder;

        public PropertyObserverMapBuilder
        (
            PropertyObserverBuilder propertyObserverBuilder
        )
        {
            _PropertyObserverBuilder = propertyObserverBuilder;
        }

        public PropertyObserverMap BuildFor(ObjectObserver oParent)
        {
            var tClass = oParent.Template;

            var visibility = tClass.Attributes.Get<VisibilityAttribute>();
            if (!visibility.IsAllowed)
                return new PropertyObserverMap();

            var results = new Dictionary<string, BasePropertyObserver>();

            foreach (var tProp in tClass.Properties.Values)
            {
                if (tProp.IsVisible == false)
                    continue;

                var oProperty = _PropertyObserverBuilder.BuildFor(oParent, tProp);

                oProperty.FinaliseConstruction();

                results.Add(tProp.Name, oProperty);
            }

            return new PropertyObserverMap(results);
        }
    }
}