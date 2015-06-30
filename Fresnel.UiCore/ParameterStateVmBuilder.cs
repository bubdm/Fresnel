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
        private ObserverRetriever _ObserverRetriever;
        private BooleanValueFormatter _BooleanValueFormatter;
        private DateTimeValueFormatter _DateTimeValueFormatter;

        public ParameterStateVmBuilder
            (
            ObserverRetriever observerRetriever,
            BooleanValueFormatter booleanValueFormatter,
            DateTimeValueFormatter dateTimeValueFormatter
            )
        {
            _ObserverRetriever = observerRetriever;
            _BooleanValueFormatter = booleanValueFormatter;
            _DateTimeValueFormatter = dateTimeValueFormatter;
        }
        
        public ValueStateVM BuildFor(ParameterObserver oParam)
        {
            var result = this.BuildFor(oParam.Template, oParam.RealObject);
            return result;
        }

        public ValueStateVM BuildFor(ParameterTemplate tParam, object realValue)
        {
            var result = new ValueStateVM();

            result.ValueType = tParam.ParameterType.Name;
            result.Get = this.BuildGet(tParam);
            result.Set = this.BuildSet(tParam);

            try
            {
                if (realValue == null)
                {
                    result.Value = realValue;
                }
                else if (tParam.IsDomainObject || tParam.IsCollection)
                {
                    var oValue = _ObserverRetriever.GetObserver(realValue, tParam.ParameterType);
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

                result.FriendlyValue = this.CreateFriendlyValue(tParam, realValue);

                // Hack:
                if (tParam.ParameterType.IsEnum &&
                    realValue != null)
                {
                    result.Value = (int)result.Value;
                }
            }
            catch (Exception ex)
            {
                result.Get.Error = ex.Message;
            }

            result.Create = tParam.IsCollection ? this.BuildCreateForCollection(tParam, result) :
                            tParam.IsDomainObject ? this.BuildCreateForObject(tParam, result) :
                            null;

            result.Clear = this.BuildClear(tParam, result);

            result.Add = tParam.IsCollection ? 
                            this.BuildAdd(tParam, result) :
                            null;

            return result;
        }

        private InteractionPoint BuildGet(ParameterTemplate tParam)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = true,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildSet(ParameterTemplate tParam)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = tParam.IsDomainObject || tParam.IsCollection,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildCreateForCollection(ParameterTemplate tParam, ValueStateVM valueState)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = true,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildCreateForObject(ParameterTemplate tParam, ValueStateVM valueState)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = true,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildClear(ParameterTemplate tParam, ValueStateVM valueState)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = true,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildAdd(ParameterTemplate tParam, ValueStateVM valueState)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = tParam.IsCollection,
                IsVisible = false
            };
            return result;
        }
        
        private string CreateFriendlyValue(ParameterTemplate tParam, object value)
        {
            if (tParam.IsCollection)
                return "...";

            if (value is bool)
            {
                return _BooleanValueFormatter.GetFriendlyValue((bool)value, tParam.Attributes);
            }

            if (value is DateTime)
            {
                return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, tParam.Attributes);
            }

            var paramType = tParam.ParameterType;
            if (paramType.IsEnum)
            {
                if (((EnumTemplate)tParam.InnerClass).IsBitwiseEnum)
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