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
        private FriendlyDisplayValueCreator _FriendlyDisplayValueCreator;

        public ParameterStateVmBuilder
            (
            ObserverRetriever observerRetriever,
            FriendlyDisplayValueCreator friendlyDisplayValueCreator
            )
        {
            _ObserverRetriever = observerRetriever;
            _FriendlyDisplayValueCreator = friendlyDisplayValueCreator;
        }

        public ValueStateVM BuildFor(ParameterObserver oParam)
        {
            var result = this.BuildFor(oParam.Template, oParam.Value);
            return result;
        }

        public ValueStateVM BuildFor(ParameterTemplate tParam, object realValue)
        {
            var result = new ValueStateVM();

            result.ValueType = tParam.ParameterType.Name;

            try
            {
                if (realValue == null)
                {
                    result.Value = null;
                }
                else if (tParam.IsDomainObject || tParam.IsCollection)
                {
                    var oValue = _ObserverRetriever.GetObserver(realValue, tParam.ParameterType);
                    result.ReferenceValueID = oValue.ID;
                }
                else
                {
                    result.Value = realValue;
                }

                result.FriendlyValue = _FriendlyDisplayValueCreator.Create(tParam, realValue);

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

            result.Get = this.BuildGet(tParam);

            result.Set = this.BuildSet(tParam, result);

            result.Create = tParam.IsCollection ? this.BuildCreateForCollection(tParam, result) :
                            tParam.IsDomainObject ? this.BuildCreateForObject(tParam, result) :
                            null;

            result.Clear = this.BuildClear(tParam, result);

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
            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;

            var result = new InteractionPoint()
            {
                IsEnabled = !isNull,
                IsVisible = true
            };
            return result;
        }

        private InteractionPoint BuildSet(ParameterTemplate tParam, ValueStateVM valueState)
        {
            var result = new InteractionPoint()
            {
                IsEnabled = true,
                IsVisible = true
            };
            return result;
        }

        //private string CreateFriendlyValue(ParameterTemplate tParam, object value)
        //{
        //    if (tParam.IsCollection)
        //        return "...";

        //    if (value is bool)
        //    {
        //        return _FriendlyDisplayValueCreator.GetFriendlyValue((bool)value, tParam.Attributes);
        //    }

        //    if (value is DateTime)
        //    {
        //        return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, tParam.Attributes);
        //    }

        //    var paramType = tParam.ParameterType;
        //    if (paramType.IsEnum)
        //    {
        //        if (((EnumTemplate)tParam.InnerClass).IsBitwiseEnum)
        //        {
        //            var enumValue = Enum.ToObject(paramType, value);
        //            return (int)enumValue == 0 ?
        //                    "" :
        //                    Enum.Format(paramType, enumValue, "G");
        //        }
        //        else
        //        {
        //            var enumValue = Enum.ToObject(paramType, Convert.ToInt64(value));
        //            return Enum.Format(paramType, enumValue, "G");
        //        }
        //    }

        //    return value == null ? null : value.ToString();
        //}

    }
}