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
        private CanClearPermission _CanClearPermission;

        private ObserverRetriever _ObserverRetriever;
        private BooleanValueFormatter _BooleanValueFormatter;
        private DateTimeValueFormatter _DateTimeValueFormatter;

        public PropertyStateVmBuilder
            (
            CanCreatePermission canCreatePermission,
            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission,
            CanClearPermission canClearPermission,
            ObserverRetriever observerRetriever,
            BooleanValueFormatter booleanValueFormatter,
            DateTimeValueFormatter dateTimeValueFormatter
            )
        {
            _CanCreatePermission = canCreatePermission;
            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
            _CanClearPermission = canClearPermission;
            _ObserverRetriever = observerRetriever;
            _BooleanValueFormatter = booleanValueFormatter;
            _DateTimeValueFormatter = dateTimeValueFormatter;
        }

        public ValueStateVM BuildFor(BasePropertyObserver oProp)
        {
            Exception getPropertyException = null;
            object realValue = null;

            var tProp = oProp.Template;

            try
            {
                var isSafeToRead = true;
                var oObjectProp = oProp as ObjectPropertyObserver;

                if (oObjectProp != null && oProp.OuterObject.ChangeTracker.IsTransient)
                {
                    // The property is in-memory, so it's safe to access:
                    oObjectProp.IsLazyLoaded = true;
                    isSafeToRead = true;
                }
                else if (oObjectProp != null && oObjectProp.IsLazyLoadPending)
                {
                    // Do nothing - We don't want to accidentally trigger a lazy load
                    isSafeToRead = false;
                }

                if (isSafeToRead)
                {
                    realValue = tProp.GetProperty(oProp.OuterObject.RealObject);
                }
            }
            catch (Exception ex)
            {
                getPropertyException = ex;
            }

            var result = this.BuildFor(tProp, realValue);
            if (getPropertyException != null)
            {
                result.Get.Error = getPropertyException.Message;
            }
            return result;
        }

        public ValueStateVM BuildFor(PropertyTemplate tProp, object realValue)
        {
            var result = new ValueStateVM();

            result.Get = this.BuildGetForNulls(tProp);
            result.Set = this.BuildSet(tProp);

            if (result.Get.IsEnabled)
            {
                try
                {
                    if (realValue == null)
                    {
                        result.Value = realValue;
                    }
                    else if (!tProp.IsNonReference)
                    {
                        var oValue = _ObserverRetriever.GetObserver(realValue, tProp.PropertyType);
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

                    result.FriendlyValue = this.CreateFriendlyValue(tProp, realValue);

                    // Hack:
                    if (tProp.PropertyType.IsEnum &&
                        realValue != null)
                    {
                        result.Value = (int)result.Value;
                    }
                }
                catch (Exception ex)
                {
                    result.Get.Error = ex.Message;
                }
            }

            // The property may have a null value, so replace the "Get" as necessary:
            result.Get = this.BuildGetForNulls(tProp, result.Get, result);

            result.Create = tProp.IsCollection ? this.BuildCreateForCollection(tProp, result) :
                            tProp.IsDomainObject ? this.BuildCreateForObject(tProp, result) :
                            null;

            result.Clear = this.BuildClear(tProp, result);

            result.Add = tProp.IsCollection ? this.BuildAdd(tProp, result) :
                            null;

            return result;
        }

        private InteractionPoint BuildCreateForObject(PropertyTemplate tProp, ValueStateVM valueState)
        {
            var result = new InteractionPoint();

            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;
            if (!isNull)
            {
                result.IsEnabled = false;
                result.IsVisible = false;
                result.Error = "You must disassociate from the existing item before replacing it with a new one";
                return result;
            }

            if (!tProp.IsCompositeRelationship)
            {
                result.IsEnabled = false;
                result.IsVisible = false;
                result.Error = "New items can only be created if the parent object is the Owner";
                return result;
            }

            var tClassType = (ClassTemplate)tProp.InnerClass;
            var createCheck = _CanCreatePermission.IsSatisfiedBy(tClassType);
            result.IsEnabled = createCheck == null;
            result.Error = result.IsEnabled ? null : createCheck.ToSingleMessage();
            return result;
        }

        private InteractionPoint BuildCreateForCollection(PropertyTemplate tProp, ValueStateVM valueState)
        {
            var result = new InteractionPoint();

            if (!tProp.IsCompositeRelationship)
            {
                result.IsEnabled = false;
                result.IsVisible = false;
                result.Error = "New items can only be created if the parent object is the Owner";
                return result;
            }

            var tClassType = ((CollectionTemplate)tProp.InnerClass).InnerClass;
            var createCheck = _CanCreatePermission.IsSatisfiedBy(tClassType);
            result.IsEnabled = createCheck == null;
            result.Error = result.IsEnabled ? null : createCheck.ToSingleMessage();
            return result;
        }

        private InteractionPoint BuildGetForNulls(PropertyTemplate tProp)
        {
            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(tProp);

            var result = new InteractionPoint()
            {
                IsEnabled = getCheck == null,
                Error = getCheck == null ? null : getCheck.ToSingleMessage(),
            };
            return result;
        }

        private InteractionPoint BuildGetForNulls(PropertyTemplate tProp, InteractionPoint existingGetAction, ValueStateVM valueState)
        {
            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;

            if (isNull && tProp.IsReferenceType)
            {
                var result = new InteractionPoint()
                {
                    IsEnabled = false,
                    Error = string.Concat("Property '", tProp.FriendlyName, "' does not have any value (it is null)")
                };
                return result;
            }
            else
            {
                return existingGetAction;
            }
        }

        private InteractionPoint BuildSet(PropertyTemplate oProp)
        {
            var setCheck = _CanSetPropertyPermission.IsSatisfiedBy(oProp);

            var result = new InteractionPoint()
            {
                IsEnabled = setCheck == null,
                Error = setCheck == null ? null : setCheck.ToSingleMessage()
            };
            return result;
        }

        private InteractionPoint BuildClear(PropertyTemplate tProp, ValueStateVM valueState)
        {
            var clearCheck = _CanClearPermission.IsSatisfiedBy(tProp);

            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;

            var result = new InteractionPoint()
            {
                IsEnabled = clearCheck == null &&
                            !isNull,
            };
            return result;
        }

        private InteractionPoint BuildAdd(PropertyTemplate tProp, ValueStateVM valueState)
        {
            var isNull = valueState.ReferenceValueID == null && valueState.Value == null;

            var result = new InteractionPoint()
            {
                IsEnabled = tProp.CanAdd &&
                            !isNull &&
                            tProp.IsCollection &&
                            ((CollectionTemplate)tProp.InnerClass).HasAddMethods
            };
            return result;
        }

        private string CreateFriendlyValue(PropertyTemplate tProp, object value)
        {
            if (value == null)
                return "";

            if (tProp.IsCollection)
                return "...";

            if (value is bool)
            {
                return _BooleanValueFormatter.GetFriendlyValue((bool)value, tProp.Attributes);
            }

            if (value is DateTime)
            {
                return _DateTimeValueFormatter.GetFriendlyValue((DateTime)value, tProp.Attributes);
            }

            var propertyType = tProp.PropertyType;
            if (propertyType.IsEnum)
            {
                if (((EnumTemplate)tProp.InnerClass).IsBitwiseEnum)
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