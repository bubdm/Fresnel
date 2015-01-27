using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class ObjectSelectionVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return template.IsDomainObject;
        }

        public void Populate(ValueVM targetVM, PropertyTemplate tProp, Type actualType)
        {
        }

        public void Populate(ValueVM targetVM, ParameterTemplate tParam, Type actualType)
        {
        }
    }
}