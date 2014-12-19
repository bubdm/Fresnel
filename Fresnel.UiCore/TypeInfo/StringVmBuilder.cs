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
    public class StringVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            return actualType == typeof(char) ||
                   actualType == typeof(string);
        }

        
        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            var fileAttr = oProp.Template.Attributes.Get<FilePathAttribute>();
            var textAttr = oProp.Template.Attributes.Get<TextAttribute>();

            targetVM.Info = new StringVM()
            {
                MinLength = textAttr.MinLength,
                MaxLength = actualType == typeof(char) ? 1 : textAttr.MaxLength,
                IsMultiLine = textAttr.IsMultiLine,
                IsPassword = textAttr.IsPassword,
                EditMask = textAttr.EditMask
            };
        }

        public string GetFormattedValue(BasePropertyObserver oProp, object realPropertyValue)
        {
            return realPropertyValue.ToStringOrNull();
        }

    }
}
