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
    public class EnumVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType.IsEnum;
        }

        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            var tEnum = (EnumTemplate)oProp.Template.InnerClass;
            var attr = oProp.Template.Attributes.Get<EnumAttribute>();

            targetVM.Info = new EnumVM()
            {
                Name = "enum",
                PreferredControl = attr.PreferredInputControl != InputControlTypes.None ?
                                   attr.PreferredInputControl :
                                   InputControlTypes.Radio
            };
        }

        public string GetFormattedValue(BasePropertyObserver oProp, object realPropertyValue)
        {
            return realPropertyValue.ToStringOrNull();
        }

    }
}
