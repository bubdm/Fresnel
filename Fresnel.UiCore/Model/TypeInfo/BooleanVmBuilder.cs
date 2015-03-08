using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;

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
            var attr = tProp.Attributes.Get<BooleanConfiguration>();

            targetVM.Info = this.CreateInfoVM(tClass, attr);
        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tClass = tParam.InnerClass;
            var attr = tParam.Attributes.Get<BooleanConfiguration>();

            targetVM.Info = this.CreateInfoVM(tClass, attr);
        }

        private ITypeInfo CreateInfoVM(IClassTemplate tClass, BooleanConfiguration attr)
        {
            return new BooleanVM()
            {
                Name = "boolean",
                IsNullable = tClass.RealType.IsNullableType(),
                TrueValue = attr.TrueValue,
                FalseValue = attr.FalseValue,
                PreferredControl = attr.PreferredInputControl != UiControlType.None ?
                                   attr.PreferredInputControl :
                                   UiControlType.Radio
            };
        }

    }
}