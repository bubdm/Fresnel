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
    // TODO: This is very similar to PropertyStateVmBuilder. Refactor for DRY.

    public class ParameterStateVmBuilder
    {
        private ObserverCache _ObserverCache;
        private BooleanValueFormatter _BooleanValueFormatter;
        private DateTimeValueFormatter _DateTimeValueFormatter;

        public ParameterStateVmBuilder
            (
            ObserverCache observerCache,
            BooleanValueFormatter booleanValueFormatter,
            DateTimeValueFormatter dateTimeValueFormatter
            )
        {
            _ObserverCache = observerCache;
            _BooleanValueFormatter = booleanValueFormatter;
            _DateTimeValueFormatter = dateTimeValueFormatter;
        }

        public ValueStateVM BuildFor(ParameterObserver oParam)
        {
            var result = new ValueStateVM();

            try
            {
                var realValue = oParam.Value;

                if (realValue == null)
                {
                    result.Value = realValue;
                }
                else if (oParam.IsObject || oParam.IsCollection)
                {
                    var oValue = _ObserverCache.GetObserver(realValue, oParam.Template.ParameterType);
                    result.ReferenceValueID = oValue.ID;
                }
                else if (realValue is bool)
                {
                    result.Value = _BooleanValueFormatter.GetValue((bool)realValue);
                }
                else if (realValue is DateTime)
                {
                    result.Value = _DateTimeValueFormatter.GetValue((DateTime)realValue);
                }
                else
                {
                    result.Value = realValue;
                }

                result.FriendlyValue = this.CreateFriendlyValue(oParam, realValue);

                // Hack:
                if (oParam.Template.ParameterType.IsEnum)
                {
                    result.Value = (int)result.Value;
                }
            }
            catch (Exception ex)
            {
                result.Get.Error = ex.Message;
            }

            return result;
        }
        
        private string CreateFriendlyValue(ParameterObserver oParam, object value)
        {
            if (oParam.Template.IsCollection)
                return "...";

            if (value is bool)
            {
                return _BooleanValueFormatter.GetFriendlyValue((bool)value, oParam.Template.Attributes);
            }

            if (value is DateTime)
            {
                return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, oParam.Template.Attributes);
            }

            var paramType = oParam.Template.ParameterType;
            if (paramType.IsEnum)
            {
                if (((EnumTemplate)oParam.Template.InnerClass).IsBitwiseEnum)
                {
                    var enumValue = Enum.ToObject(paramType, value);
                    return (int)enumValue == 0 ?
                            "" :
                            Enum.Format(paramType, enumValue, "G");
                }
                else
                {
                    var enumValue = Enum.ToObject(paramType, Convert.ToInt64(value));
                    return Enum.Format(paramType, enumValue, "G");
                }
            }

            return value == null ? null : value.ToString();
        }

    }
}