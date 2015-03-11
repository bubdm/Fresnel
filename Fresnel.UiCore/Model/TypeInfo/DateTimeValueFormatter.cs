using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class DateTimeValueFormatter
    {
        public object GetValue(DateTime value)
        {
            return value.ToString("s");
        }

        public string GetFriendlyValue(DateTime value, AttributesMap attributes)
        {
            var displayFormat = attributes.Get<DisplayFormatAttribute>();

            var result = displayFormat.DataFormatString.IsNotEmpty() ?
                            value.ToString(displayFormat.DataFormatString) :
                            value.ToString();

            return result;
        }

    }
}