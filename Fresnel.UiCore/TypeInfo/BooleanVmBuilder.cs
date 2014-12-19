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
    public class BooleanVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(bool);
        }

        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            var attr = oProp.Template.Attributes.Get<BooleanAttribute>();

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
