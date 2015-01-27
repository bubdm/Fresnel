using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public interface IPropertyVmBuilder
    {
        bool CanHandle(ISettableMemberTemplate template,Type actualType);

        void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType);

        void Populate(MethodParameterVM targetVM, ParameterTemplate tParam, Type actualType);
        
    }
}