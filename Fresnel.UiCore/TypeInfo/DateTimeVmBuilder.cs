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
    public class DateTimeVmBuilder : ITypeInfoBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(DateTime) ||
                   actualType == typeof(DateTimeOffset);
        }

        public ITypeInfo BuildTypeInfoFor(BasePropertyObserver oProp, Type actualType)
        {
            var attr = oProp.Template.Attributes.Get<DateTimeAttribute>();

            var result = new DateTimeVM()
            {
                 CustomFormat = attr.CustomFormat,
            };
            return result;
        }
    }
}
