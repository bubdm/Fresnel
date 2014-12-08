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
        public bool CanHandle(BasePropertyObserver oProp)
        {
            var tClass = oProp.Template.InnerClass; 
            return tClass.RealType == typeof(Int16) ||
                   tClass.RealType == typeof(Int32) ||
                   tClass.RealType == typeof(Int64) ||
                   tClass.RealType == typeof(byte);
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp)
        {
            return EditorType.Integer;
        }
    }
}
