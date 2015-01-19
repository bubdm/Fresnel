using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class StringVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;
            return actualType == typeof(char) ||
                   actualType == typeof(string);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;
            var attr = tProp.Attributes.Get<TextAttribute>();

            targetVM.Info = new StringVM()
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