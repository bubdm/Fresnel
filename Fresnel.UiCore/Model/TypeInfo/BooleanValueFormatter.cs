using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class BooleanValueFormatter
    {
        public object GetValue(bool value)
        {
            return value;
        }

        public string GetFriendlyValue(bool value, AttributesMap attributes)
        {
            var displayBoolean = attributes.Get<DisplayBooleanAttribute>();
            var result = value ? displayBoolean.TrueValue : displayBoolean.FalseValue;
            return result;
        }

    }
}