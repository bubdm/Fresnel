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
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class PropertyStateVmBuilder
    {
        private CanCreatePermission _CanCreatePermission;
        private CanGetPropertyPermission _CanGetPropertyPermission;
        private CanSetPropertyPermission _CanSetPropertyPermission;

        public PropertyStateVmBuilder
            (
            CanCreatePermission canCreatePermission,
            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission
            )
        {
            _CanCreatePermission = canCreatePermission;
            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
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
                    result.Value = realValue;
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
                result.Create = this.BuildCreate(oProp, result.Value);
            }

            result.Clear = this.BuildClear(oProp, result.Value);

            if (oProp.Template.IsCollection)
            {
                result.Add = this.BuildAdd(oProp, result.Value);
            }

            return result;
        }

        private InteractionPoint BuildCreate(BasePropertyObserver oProp, object propertyValue)
        {
            var result = new InteractionPoint();

            if (propertyValue == null)
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

        private InteractionPoint BuildClear(BasePropertyObserver oProp, object propertyValue)
        {
            var tProp = oProp.Template;
            var result = new InteractionPoint()
            {
                IsEnabled = tProp.CanWrite &&
                             propertyValue != null &&
                             (!tProp.IsNonReference || tProp.IsNullableType),
            };
            return result;
        }

        private InteractionPoint BuildAdd(BasePropertyObserver oProp, object propertyValue)
        {
            var tProp = oProp.Template;
            var result = new InteractionPoint()
            {
                IsEnabled = tProp.CanAdd &&
                            propertyValue != null &&
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
                var attr = oProp.Template.Attributes.Get<BooleanAttribute>();
                var result = (bool)value ? attr.TrueValue : attr.FalseValue;
                return result;
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

        //private string ConvertToJavascriptType(Type type)
        //{
        //    switch (type.Name.ToLower())
        //    {
        //        case "boolean":
        //            return "boolean";

        //        case "datetime":
        //        case "datetimeoffset":
        //            return "date";

        //        case "decimal":
        //        case "double":
        //        case "single":
        //        case "int32":
        //        case "uint32":
        //        case "int64":
        //        case "uint64":
        //        case "int16":
        //        case "uint16":
        //            return "number";

        //        case "string":
        //        case "char":
        //            return "string";

        //        default:
        //            return "object";
        //    }
        //}
    }
}