using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Types;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class AbstractPropertyVmBuilder
    {
        private List<IPropertyVmBuilder> _Builders;
        private UnknownVmBuilder _UnknownVmBuilder;

        private CanGetPropertyPermission _CanGetPropertyPermission;
        private CanSetPropertyPermission _CanSetPropertyPermission;
        private RealTypeResolver _RealTypeResolver;

        public AbstractPropertyVmBuilder
            (
            BooleanVmBuilder booleanVmBuilder,
            DateTimeVmBuilder dateTimeVmBuilder,
            EnumVmBuilder enumVmBuilder,
            NumberVmBuilder numberVmBuilder,
            StringVmBuilder textVmBuilder,
            ObjectSelectionVmBuilder objectSelectionVmBuilder,
            UnknownVmBuilder unknownVmBuilder,

            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission,
            RealTypeResolver realTypeResolver
            )
        {
            _Builders = new List<IPropertyVmBuilder>()
            {
                booleanVmBuilder ,
                dateTimeVmBuilder,
                enumVmBuilder,
                numberVmBuilder,
                textVmBuilder,
                objectSelectionVmBuilder,
            };
            _UnknownVmBuilder = unknownVmBuilder;

            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
            _RealTypeResolver = realTypeResolver;
        }

        public PropertyVM BuildFor(PropertyTemplate tProp)
        {
            var valueType = tProp.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var propVM = new PropertyVM()
            {
                Name = tProp.FriendlyName,
                PropertyName = tProp.Name,
                Description = tProp.XmlComments.Summary,
                IsRequired = tProp.IsNonReference && !tProp.IsNullableType,
                IsVisible = !tProp.IsFrameworkMember && tProp.IsVisible,
                JavascriptType = this.ConvertToJavascriptType(actualType),
            };

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(tProp, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(propVM, tProp, actualType);

            return propVM;
        }

        public PropertyVM BuildFor(BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;
            var objectProp = oProp as ObjectPropertyObserver;

            var setCheck = _CanSetPropertyPermission.IsSatisfiedBy(oProp);
            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);

            var propVM = this.BuildFor(tProp);
            propVM.ObjectID = oProp.OuterObject.ID;
            propVM.IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true;
            propVM.IsExpandable = objectProp != null;
            propVM.CanRead = getCheck.Passed;
            propVM.CanWrite = setCheck.Passed;

            if (setCheck.Passed)
            {
                propVM.IsEnabled = true;
            }
            else
            {
                propVM.Warning = setCheck.FailureReason;
            }

            if (getCheck.Passed)
            {
                try
                {
                    // TODO: Use the GetPropertyCommand, in case the property should be hidden:
                    var realValue = oProp.Template.GetProperty(oProp.OuterObject.RealObject);
                    propVM.Value = realValue;
                }
                catch (Exception ex)
                {
                    propVM.Error = ex.Message;
                }
            }
            else
            {
                propVM.Error = getCheck.FailureReason;
            }

            return propVM;
        }

        private string ConvertToJavascriptType(Type type)
        {
            switch (type.Name.ToLower())
            {
                case "boolean":
                    return "Boolean";

                case "datetime":
                case "datetimeoffset":
                    return "Date";

                case "decimal":
                case "double":
                case "single":
                case "int32":
                case "uint32":
                case "int64":
                case "uint64":
                case "int16":
                case "uint16":
                    return "number";

                case "string":
                case "char":
                    return "string";

                default:
                    return "Object";
            }
        }
    }
}
