using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using System;

namespace Envivo.Fresnel.UiCore.Types
{
    public class UnknownVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            return false;
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            targetVM.CanWrite = false;
        }
    }
}