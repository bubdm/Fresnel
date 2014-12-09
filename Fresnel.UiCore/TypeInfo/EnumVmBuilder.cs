using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class EnumVmBuilder : ITypeInfoBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType.IsEnum;
        }

        public ITypeInfo BuildTypeInfoFor(BasePropertyObserver oProp, Type actualType)
        {
            var tEnum = (EnumTemplate)oProp.Template.InnerClass;
            var enumAttr = oProp.Template.Attributes.Get<EnumAttribute>();

            var result = new EnumVM()
            {
                PreferredUiControl = enumAttr.PreferredUiControl
            };

            return result;
        }
    }
}
