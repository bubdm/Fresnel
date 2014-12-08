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
    public class TextIdentifier : IEditorTypeIdentifier
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            return actualType == typeof(char) ||
                   actualType == typeof(string);
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp, Type actualType)
        {
            var tClass = oProp.Template.InnerClass;
            var fileAttr = oProp.Template.Attributes.Get<FilePathAttribute>();
            var textAttr = oProp.Template.Attributes.Get<TextAttribute>();

            if (fileAttr.DialogType != FileDialogType.None)
            {
                return EditorType.FileDialog;
            }
            else if (textAttr.EditMask.IsNotEmpty())
            {
                return EditorType.MaskedString;
            }
            else if (textAttr.IsRichText)
            {
                return EditorType.MarkdownText;
            }
            else if (textAttr.IsMultiLine)
            {
                return EditorType.MultiLineText;
            }
            else if (textAttr.IsPassword)
            {
                return EditorType.Password;
            }
            else if (actualType == typeof(char))
            {
                return EditorType.Character;
            }

            return EditorType.String;
        }
    }
}
