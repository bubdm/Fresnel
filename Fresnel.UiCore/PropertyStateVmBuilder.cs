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
        private CanGetPropertyPermission _CanGetPropertyPermission;
        private CanSetPropertyPermission _CanSetPropertyPermission;

        public PropertyStateVmBuilder
            (
            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission
            )
        {
            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
        }

        public ValueStateVM BuildFor(BasePropertyObserver oProp)
        {
            var result = new ValueStateVM();
            result.Get = this.CreateGet(oProp);
            result.Set = this.CreateSet(oProp);

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

            result.Clear = this.CreateClear(oProp, result.Value);

            if (oProp.Template.IsCollection)
            {
                result.Add = this.CreateAdd(oProp, result.Value);
            }

            return result;
        }

        private InteractionPoint CreateGet(BasePropertyObserver oProp)
        {
            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);

            var result = new InteractionPoint()
            {
                IsEnabled = getCheck.Passed,
                Error = getCheck.FailureReason,
            };
            return result;
        }

        private InteractionPoint CreateSet(BasePropertyObserver oProp)
        {
            var setCheck = _CanSetPropertyPermission.IsSatisfiedBy(oProp);

            var result = new InteractionPoint()
            {
                IsEnabled = setCheck.Passed,
                Error = setCheck.FailureReason,
            };
            return result;
        }

        private InteractionPoint CreateClear(BasePropertyObserver oProp, object propertyValue)
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

        private InteractionPoint CreateAdd(BasePropertyObserver oProp, object propertyValue)
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