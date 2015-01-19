using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class ObjectSelectionVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;
            return tClass is ClassTemplate;
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
        }
    }
}