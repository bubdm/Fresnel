using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class NumberVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(double) ||
                   actualType == typeof(float) ||
                   actualType == typeof(decimal) ||
                   actualType == typeof(Int16) ||
                   actualType == typeof(Int32) ||
                   actualType == typeof(Int64) ||
                   actualType == typeof(byte);
        }


        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            var attr = oProp.Template.Attributes.Get<NumberAttribute>();

            targetVM.Info = new NumberVM()
            {
                Name = "number",
                MinValue = attr.MinValue,
                MaxValue = attr.MaxValue,
                DecimalPlaces = attr.DecimalPlaces,
                CurrencySymbol = "",
                PreferredControl = attr.IsCurrency ? InputControlTypes.Currency :
                                   attr.PreferredInputControl != InputControlTypes.None ?
                                   attr.PreferredInputControl :
                                   InputControlTypes.Number
            };
        }

        public string GetFormattedValue(BasePropertyObserver oProp, object realPropertyValue)
        {
            return realPropertyValue.ToStringOrNull();
        }

    }
}
