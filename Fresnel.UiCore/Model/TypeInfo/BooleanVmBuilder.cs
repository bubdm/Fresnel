using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class BooleanVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(bool);
        }

        public void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;

            targetVM.Info = this.CreateInfoVM(tProp.Attributes, tClass.RealType);
        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tClass = tParam.InnerClass;
            
            targetVM.Info = this.CreateInfoVM(tParam.Attributes, tClass.RealType);
        }

        private ITypeInfo CreateInfoVM(AttributesMap attributesMap, Type actualType)
        {
            var displayBoolean = attributesMap.Get<DisplayBooleanAttribute>();

            var preferredControl = attributesMap.Get<UiControlHintAttribute>().PreferredUiControl;
            if (preferredControl == UiControlType.None)
            {
                preferredControl = UiControlType.Radio;
            }

            return new BooleanVM()
            {
                Name = "boolean",
                IsNullable = actualType.IsNullableType(),
                TrueValue = displayBoolean.TrueValue,
                FalseValue = displayBoolean.FalseValue,
                PreferredControl = preferredControl
            };
        }

    }
}