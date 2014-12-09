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
    public class BooleanVmBuilder : ITypeInfoBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(bool);
        }

        public ITypeInfo BuildTypeInfoFor(BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            var attr = tClass.Attributes.Get<BooleanAttribute>();

            var result = new BooleanVM()
            {
                IsNullable = tClass.RealType.IsNullableType(),
                TrueValue = attr.TrueValue,
                FalseValue = attr.FalseValue,
            };

            return result;
        }
    }
}
