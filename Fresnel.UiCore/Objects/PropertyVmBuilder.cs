using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class PropertyVmBuilder
    {
        private EditorTypeIdentifier _EditorTypeIdentifier;

        public PropertyVmBuilder
            (
            EditorTypeIdentifier editorTypeIdentifier
            )
        {
            _EditorTypeIdentifier = editorTypeIdentifier;
        }

        public PropertyVM BuildFor(ObjectObserver oObject, BasePropertyObserver oProp)
        {
            var objectProp = oProp as ObjectPropertyObserver;

            var propVM = new PropertyVM()
            {
                //ObjectID = oObject.ID, // No need to include this, as it just adds bloat:
                Name = oProp.Template.FriendlyName,
                Value = oProp.Template.GetProperty(oObject.RealObject),
                EditorType = _EditorTypeIdentifier.DetermineEditorFor(oProp),
                IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true,
                IsVisible = !oProp.Template.IsFrameworkMember && oProp.Template.IsVisible,
                IsEnabled = true,
                IsExpandable = objectProp != null,
            };

            return propVM;
        }

    }
}
