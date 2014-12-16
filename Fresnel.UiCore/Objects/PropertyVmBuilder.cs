using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.UiCore.TypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class PropertyVmBuilder
    {
        private TypeInfoBuilder _TypeInfoBuilder;
        private CanGetPropertyPermission _CanGetPropertyPermission;
        private CanSetPropertyPermission _CanSetPropertyPermission;

        public PropertyVmBuilder
            (
            TypeInfoBuilder typeInfoBuilder,
            CanGetPropertyPermission canGetPropertyPermission,
            CanSetPropertyPermission canSetPropertyPermission
            )
        {
            _TypeInfoBuilder = typeInfoBuilder;
            _CanGetPropertyPermission = canGetPropertyPermission;
            _CanSetPropertyPermission = canSetPropertyPermission;
        }

        public PropertyVM BuildFor(ObjectObserver oObject, BasePropertyObserver oProp)
        {
            var objectProp = oProp as ObjectPropertyObserver;

            var getCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);
            var setCheck = _CanGetPropertyPermission.IsSatisfiedBy(oProp);

            var propVM = new PropertyVM()
            {
                ObjectID = oObject.ID,
                Name = oProp.Template.FriendlyName,
                PropertyName = oProp.Template.Name,
                Description = oProp.Template.XmlComments.Summary,
                Info = _TypeInfoBuilder.BuildTypeInfoFor(oProp),
                IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true,
                IsVisible = !oProp.Template.IsFrameworkMember && oProp.Template.IsVisible,
                IsExpandable = objectProp != null,
            };

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
                    propVM.Value = oProp.Template.GetProperty(oObject.RealObject);
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
