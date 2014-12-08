using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Editing
{
    public class EnumIdentifier : IEditorTypeIdentifier
    {
        public bool CanHandle(BasePropertyObserver oProp)
        {
            var tClass = oProp.Template.InnerClass;
            return tClass.RealType.IsEnum;
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp)
        {
            var tEnum = (EnumTemplate)oProp.Template.InnerClass;
            if (tEnum.IsBitwiseEnum)
            {
                return EditorType.EnumCheckboxes;
            }

            var enumAttr = oProp.Template.Attributes.Get<EnumAttribute>();
            switch (enumAttr.PreferredUiControl)
            {
                case EnumEditorControl.DropDownList:
                    return EditorType.EnumDropDown;

                case EnumEditorControl.RadioOptions:
                    return EditorType.EnumRadioOptions;

                case EnumEditorControl.Slider:
                    return EditorType.EnumSlider;

                default:
                    return EditorType.EnumCheckboxes;
            }
        }
    }
}
