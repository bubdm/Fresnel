using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public interface IPropertyVmBuilder
    {
        bool CanHandle(ISettableMemberTemplate template,Type actualType);

        void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType);

        void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType);
        
    }
}