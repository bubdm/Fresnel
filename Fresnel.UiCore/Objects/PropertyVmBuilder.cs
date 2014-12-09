using Envivo.Fresnel.Core.Observers;
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

        public PropertyVmBuilder
            (
            TypeInfoBuilder typeInfoBuilder
            )
        {
            _TypeInfoBuilder = typeInfoBuilder;
        }

        public PropertyVM BuildFor(ObjectObserver oObject, BasePropertyObserver oProp)
        {
            var objectProp = oProp as ObjectPropertyObserver;

            var propVM = new PropertyVM()
            {
                //ObjectID = oObject.ID, // No need to include this, as it just adds bloat:
                Name = oProp.Template.FriendlyName,
                Description = oProp.Template.XmlComments.Summary,
                Info = _TypeInfoBuilder.BuildTypeInfoFor(oProp),
                IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true,
                IsVisible = !oProp.Template.IsFrameworkMember && oProp.Template.IsVisible,
                IsEnabled = true,
                IsExpandable = objectProp != null,
            };

            try
            {
                propVM.Value = oProp.Template.GetProperty(oObject.RealObject);
            }
            catch (Exception ex)
            {
                propVM.Error = ex.Message;
            }

            return propVM;
        }

    }
}
