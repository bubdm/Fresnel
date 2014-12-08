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
    public class ObjectSelectionIdentifier : IEditorTypeIdentifier
    {
        public bool CanHandle(BasePropertyObserver oProp)
        {
           var tClass = oProp.Template.InnerClass; 
            return tClass is ClassTemplate;
        }

        public EditorType DetermineEditorType(BasePropertyObserver oProp)
        {
            var attr = oProp.Template.Attributes.Get<ObjectPropertyAttribute>();

            var lookupSpecificationType = attr.LookupListFilter;
            if (lookupSpecificationType != null)
            {
                return EditorType.ObjectSelectionList;
            }

            return EditorType.None;
        }
    }
}
