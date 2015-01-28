using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class UnknownVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return false;
        }

        public void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType)
        {   
        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
        }
    }
}