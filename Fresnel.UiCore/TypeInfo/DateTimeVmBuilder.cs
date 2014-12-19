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
    public class DateTimeVmBuilder : IPropertyVmBuilder
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(DateTime) ||
                   actualType == typeof(DateTimeOffset);
        }

        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            var attr = oProp.Template.Attributes.Get<DateTimeAttribute>();

            targetVM.Info = new DateTimeVM()
            {
                CustomFormat = attr.CustomFormat,
                IsDateOnly = attr.IsDateOnly,
                IsTimeOnly = attr.IsTimeOnly,
            };
        }

        public string GetFormattedValue(BasePropertyObserver oProp, object realPropertyValue)
        {
            if (realPropertyValue == null)
                return null;

            var dateTime = (DateTime)realPropertyValue;


            var attr = oProp.Template.Attributes.Get<DateTimeAttribute>();
            if (attr.IsDateOnly)
            {
                return string.Empty;
                return dateTime.ToString("yyyy-MM-dd");
            }
            if (attr.IsTimeOnly)
            {
                return dateTime.ToString("T");
            }
            return string.Empty;
            return dateTime.ToString("s");
        }
    }
}
