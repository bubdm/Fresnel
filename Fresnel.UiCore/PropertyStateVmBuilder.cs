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
    public class PropertyStateVmBuilder
    {

        private CanCreatePermission _CanCreatePermission;
        private CanGetPropertyPermission _CanGetPropertyPermission;
        private CanSetPropertyPermission _CanSetPropertyPermission;

        private ObserverCache _ObserverCache;
        private BooleanValueFormatter _BooleanValueFormatter;
        private DateTimeValueFormatter _DateTimeValueFormatter;

        public PropertyStateVmBuilder
            (
            CanCreatePermission canCreatePermission,
            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission,
            ObserverCache observerCache,
            BooleanValueFormatter booleanValueFormatter,
            DateTimeValueFormatter dateTimeValueFormatter
            )
        {
            _CanCreatePermission = canCreatePermission;
            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
            _ObserverCache = observerCache;
            _BooleanValueFormatter = booleanValueFormatter;
            _DateTimeValueFormatter = dateTimeValueFormatter;
        }

        public ValueStateVM BuildFor(BasePropertyObserver oProp)
        {
            var result = new ValueStateVM();

            result.Get = this.BuildGet(oProp);
            result.Set = this.BuildSet(oProp);

            if (result.Get.IsEnabled)
            {
                try
                {
                    // TODO: Use the GetPropertyCommand, in case the property should be hidden:
                    var realValue = oProp.Template.GetProperty(oProp.OuterObject.RealObject);

                    if (realValue == null)
                    {
                        result.Value = realValue;
                    }
                    else if (oProp is ObjectPropertyObserver)
                    {
                        var oValue = _ObserverCache.GetObserver(realValue, oProp.Template.PropertyType);
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

                    result.FriendlyValue = this.CreateFriendlyValue(oProp, realValue);

                    // Hack:
                    if (oProp.Template.PropertyType.IsEnum)
                    {
                        result.Value = (int)result.Value;
                    }
                }
                catch (Exception ex)
                {
                    result.Get.Error = ex.Message;
                }
            }

            if (oProp.Template.IsDomainObject)
            {
                result.Create = this.BuildCreate(oProp, result);
            }

            result.Clear = this.BuildClear(oProp, result);

            if (oProp.Template.IsCollection)
            {
                result.Add = this.BuildAdd(oProp, result);
            }

            return result;
        }

        private InteractionPoint BuildCreate(BasePropertyObserver oProp, ValueStateVM valueState)
        {
            var result = new InteractionPoint();

            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;
            if (isNull)
            {
                var createCheck = _CanCreatePermission.IsSatisfiedBy((ClassTemplate)oProp.Template.InnerClass);
                result.IsEnabled = createCheck.Passed;
                result.Error = createCheck.FailureReason;
            }
            else
            {
                result.IsEnabled = false;
                result.IsVisible = false;
                result.Error = "You must disassociate from the existing item before replacing it with a new one";
            }

            return result;
        }

        private InteractionPoint BuildGet(BasePropertyObserver oProp)
        {
            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);

            var result = new InteractionPoint()
            {
                IsEnabled = getCheck.Passed,
                Error = getCheck.FailureReason,
            };
            return result;
        }

        private InteractionPoint BuildSet(BasePropertyObserver oProp)
        {
            var setCheck = _CanSetPropertyPermission.IsSatisfiedBy(oProp);

            var result = new InteractionPoint()
            {
                IsEnabled = setCheck.Passed,
                Error = setCheck.FailureReason,
            };
            return result;
        }

        private InteractionPoint BuildClear(BasePropertyObserver oProp, ValueStateVM valueState)
        {
            var tProp = oProp.Template;
            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;

            var result = new InteractionPoint()
            {
                IsEnabled = tProp.CanWrite &&
                             !isNull &&
                             (!tProp.IsNonReference || tProp.IsNullableType),
            };
            return result;
        }

        private InteractionPoint BuildAdd(BasePropertyObserver oProp, ValueStateVM valueState)
        {
            var tProp = oProp.Template;
            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;
            var result = new InteractionPoint()
            {
                IsEnabled = tProp.CanAdd &&
                            !isNull &&
                            tProp.IsCollection
            };
            return result;
        }

        private string CreateFriendlyValue(BasePropertyObserver oProp, object value)
        {
            if (oProp.Template.IsCollection)
                return "...";

            if (value is bool)
            {
                return _BooleanValueFormatter.GetFriendlyValue((bool)value, oProp.Template.Attributes);
            }

            if (value is DateTime)
            {
                return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, oProp.Template.Attributes);
            }

            var propertyType = oProp.Template.PropertyType;
            if (propertyType.IsEnum)
            {
                if (((EnumTemplate)oProp.Template.InnerClass).IsBitwiseEnum)
                {
                    var enumValue = Enum.ToObject(propertyType, value);
                    return (int)enumValue == 0 ?
                            "" :
                            Enum.Format(propertyType, enumValue, "G");
                }
                else
                {
                    var enumValue = Enum.ToObject(propertyType, Convert.ToInt64(value));
                    return Enum.Format(propertyType, enumValue, "G");
                }
            }

            return value == null ? null : value.ToString();
        }

    }
}