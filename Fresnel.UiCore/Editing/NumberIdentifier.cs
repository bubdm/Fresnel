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
    public class NumberIdentifier : IEditorTypeIdentifier
    {
        public bool CanHandle(BasePropertyObserver oProp)
        {
            var tClass = oProp.Template.InnerClass; 
            return tClass.RealType == typeof(double) ||
                   tClass.RealType == typeof(float) ||
                   tClass.RealType == typeof(decimal);
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp)
        {
            var numberAttr = oProp.Template.Attributes.Get<NumberAttribute>();
            if (numberAttr.IsCurrency)
            {
                return EditorType.Currency;
            }
            else
            {
                return EditorType.Number;
            }
        }
    }
}
