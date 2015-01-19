using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using System;

namespace Envivo.Fresnel.UiCore.Types
{
    public interface IPropertyVmBuilder
    {
        bool CanHandle(PropertyTemplate tProp, Type actualType);

        void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType);
    }
}