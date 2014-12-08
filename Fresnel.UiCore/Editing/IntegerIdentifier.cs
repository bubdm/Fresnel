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
    public class IntegerIdentifier : IEditorTypeIdentifier
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return actualType == typeof(Int16) ||
                   actualType == typeof(Int32) ||
                   actualType == typeof(Int64) ||
                   actualType == typeof(byte);
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp, Type actualType)
        {
            return EditorType.Integer;
        }
    }
}
