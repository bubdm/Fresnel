using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class StringVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(char) ||
                   actualType == typeof(string);
        }

        public void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;
            var attr = tProp.Attributes.Get<TextAttribute>();

            targetVM.Info = this.CreateInfoVM(attr, actualType);
        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tClass = tParam.InnerClass;
            var attr = tParam.Attributes.Get<TextAttribute>();

            targetVM.Info = this.CreateInfoVM(attr, actualType);
        }

        private ITypeInfo CreateInfoVM(TextAttribute attr, Type actualType)
        {
            return new StringVM()
            {
                Name = "string",
                MinLength = attr.MinLength,
                MaxLength = actualType == typeof(char) ? 1 : attr.MaxLength,
                EditMask = attr.EditMask,
                PreferredControl = attr.PreferredInputControl != InputControlTypes.None ?
                                   attr.PreferredInputControl :
                                   InputControlTypes.Text
            };
        }
    }
}