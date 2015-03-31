using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public interface ISettableVmBuilder
    {
        bool CanHandle(ISettableMemberTemplate template, Type actualType);

        void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType);

        void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType);

    }
}