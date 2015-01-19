using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using System;

namespace Envivo.Fresnel.UiCore.Types
{
    public class NumberVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            return actualType == typeof(double) ||
                   actualType == typeof(float) ||
                   actualType == typeof(decimal) ||
                   actualType == typeof(Int16) ||
                   actualType == typeof(Int32) ||
                   actualType == typeof(Int64) ||
                   actualType == typeof(byte);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var attr = tProp.Attributes.Get<NumberAttribute>();

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
    }
}