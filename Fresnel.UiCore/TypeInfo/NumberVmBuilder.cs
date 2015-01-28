﻿using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class NumberVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(double) ||
                   actualType == typeof(float) ||
                   actualType == typeof(decimal) ||
                   actualType == typeof(Int16) ||
                   actualType == typeof(Int32) ||
                   actualType == typeof(Int64) ||
                   actualType == typeof(byte);
        }

        public void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var attr = tProp.Attributes.Get<NumberAttribute>();

            targetVM.Info = this.CreateInfoVM(attr);

        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var attr = tParam.Attributes.Get<NumberAttribute>();

            targetVM.Info = this.CreateInfoVM(attr);
        }

        private ITypeInfo CreateInfoVM(NumberAttribute attr)
        {
            return new NumberVM()
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