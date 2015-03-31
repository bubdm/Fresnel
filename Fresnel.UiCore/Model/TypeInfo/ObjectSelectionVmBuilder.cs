using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class ObjectSelectionVmBuilder : ISettableVmBuilder
    {
        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return template.IsDomainObject;
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            targetVM.Info = new ReferenceTypeVM()
            {
                FullTypeName = tProp.InnerClass.FullName
            };
        }

        public void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            targetVM.Info = new ReferenceTypeVM()
            {
                FullTypeName = tParam.InnerClass.FullName
            };
        }
    }
}