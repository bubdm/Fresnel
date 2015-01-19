using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class BooleanVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            return actualType == typeof(bool);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;
            var attr = tProp.Attributes.Get<BooleanAttribute>();

            targetVM.Info = new BooleanVM()
            {
                Name = "boolean",
                IsNullable = tClass.RealType.IsNullableType(),
                TrueValue = attr.TrueValue,
                FalseValue = attr.FalseValue,
                PreferredControl = attr.PreferredInputControl != InputControlTypes.None ?
                                   attr.PreferredInputControl :
                                   InputControlTypes.Radio
            };
        }
    }
}