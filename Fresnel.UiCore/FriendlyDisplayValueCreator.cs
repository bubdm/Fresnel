using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class FriendlyDisplayValueCreator
    {
        private BooleanValueFormatter _BooleanValueFormatter;
        private DateTimeValueFormatter _DateTimeValueFormatter;

        public FriendlyDisplayValueCreator
            (
            BooleanValueFormatter booleanValueFormatter,
            DateTimeValueFormatter dateTimeValueFormatter
            )
        {
            _BooleanValueFormatter = booleanValueFormatter;
            _DateTimeValueFormatter = dateTimeValueFormatter;
        }

        public string Create(ISettableMemberTemplate tSettable, object value)
        {
            if (value == null)
                return "";

            if (tSettable.IsCollection)
                return "...";

            if (value is bool)
            {
                return _BooleanValueFormatter.GetFriendlyValue((bool)value, tSettable.Attributes);
            }

            if (value is DateTime)
            {
                return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, tSettable.Attributes);
            }

            var valueType = tSettable.ValueType;
            if (valueType.IsEnum)
            {
                if (((EnumTemplate)tSettable.InnerClass).IsBitwiseEnum)
                {
                    var enumValue = Enum.ToObject(valueType, value);
                    return (int)enumValue == 0 ?
                            "" :
                            Enum.Format(valueType, enumValue, "G");
                }
                else
                {
                    var enumValue = Enum.ToObject(valueType, Convert.ToInt64(value));
                    return Enum.Format(valueType, enumValue, "G");
                }
            }

            var result = value == null ? 
                                  null : 
                                  value.ToString();
            return result;
        }

    }
}