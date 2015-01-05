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

        public PropertyVM BuildFor(BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;
            var objectProp = oProp as ObjectPropertyObserver;

            var valueType = tProp.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var setCheck = _CanSetPropertyPermission.IsSatisfiedBy(oProp);
            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);

            var propVM = new PropertyVM()
            {
                ObjectID = oProp.OuterObject.ID,
                Name = tProp.FriendlyName,
                PropertyName = tProp.Name,
                Description = tProp.XmlComments.Summary,
                IsRequired = tProp.IsNonReference && !tProp.IsNullableType,
                IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true,
                IsVisible = !tProp.IsFrameworkMember && tProp.IsVisible,
                IsExpandable = objectProp != null,
                CanRead = getCheck.Passed,
                CanWrite = setCheck.Passed,
            };

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(oProp, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(propVM, oProp, actualType);

            if (setCheck.Passed)
            {
                propVM.IsEnabled = true;
            }
            else
            {
                propVM.Error = setCheck.FailureReason;
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


    }
}
