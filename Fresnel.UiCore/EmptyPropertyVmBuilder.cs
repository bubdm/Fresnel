using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class EmptyPropertyVmBuilder
    {
        private IEnumerable<ISettableVmBuilder> _Builders;
        private UnknownVmBuilder _UnknownVmBuilder;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;

        public EmptyPropertyVmBuilder
            (
            IEnumerable<ISettableVmBuilder> builders,
            ClassHierarchyBuilder classHierarchyBuilder
            )
        {
            _Builders = builders;
            _UnknownVmBuilder = builders.OfType<UnknownVmBuilder>().Single();
            _ClassHierarchyBuilder = classHierarchyBuilder;
        }

        public PropertyVM BuildFor(PropertyTemplate tProp)
        {
            var valueType = tProp.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var propVM = new PropertyVM()
            {
                Name = tProp.FriendlyName,
                InternalName = tProp.Name,
                Description = tProp.XmlComments.Summary,
                IsRequired = tProp.IsNonReference && !tProp.IsNullableType,
                IsVisible = !tProp.IsFrameworkMember && tProp.IsVisible,
                IsNonReference = tProp.IsNonReference,
                IsObject = !tProp.IsNonReference && !tProp.IsCollection,
                IsCollection = tProp.IsCollection,
            };

            if (tProp.IsDomainObject)
            {
                propVM.AllowedClassTypes = _ClassHierarchyBuilder
                                            .GetCompleteTree((ClassTemplate)tProp.InnerClass)
                                            .Select(t => t.FullName)
                                            .ToArray();
            }

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(tProp, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(propVM, tProp, actualType);

            return propVM;
        }

    }
}