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
    public class NumberVmBuilder : ITypeInfoBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(double) ||
                   actualType == typeof(float) ||
                   actualType == typeof(decimal) ||
                   actualType == typeof(Int16) ||
                   actualType == typeof(Int32) ||
                   actualType == typeof(Int64) ||
                   actualType == typeof(byte);
        }

        public ITypeInfo BuildTypeInfoFor(BasePropertyObserver oProp, Type actualType)
        {
            var numberAttr = oProp.Template.Attributes.Get<NumberAttribute>();

            var result = new NumberVM()
            {
                MinValue = numberAttr.MinValue,
                MaxValue = numberAttr.MaxValue,
                DecimalPlaces = numberAttr.DecimalPlaces,
                CurrencySymbol = ""
            };
            return result;
        }
    }
}
