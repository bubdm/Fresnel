using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class UnknownVmBuilder : ISettableVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return false;
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {   
        }

        public void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType)
        {
        }
    }
}