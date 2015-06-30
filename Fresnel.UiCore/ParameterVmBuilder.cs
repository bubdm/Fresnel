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
    public class ParameterVmBuilder
    {
        private IEnumerable<ISettableVmBuilder> _Builders;
        private ParameterStateVmBuilder _ParameterStateVmBuilder;
        private UnknownVmBuilder _UnknownVmBuilder;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;

        public ParameterVmBuilder
            (
            IEnumerable<ISettableVmBuilder> builders,
            ClassHierarchyBuilder classHierarchyBuilder
            ,
            ParameterStateVmBuilder parameterStateVmBuilder)
        {
            _Builders = builders;
            _ParameterStateVmBuilder = parameterStateVmBuilder;
            _UnknownVmBuilder = builders.OfType<UnknownVmBuilder>().Single();
            _ClassHierarchyBuilder = classHierarchyBuilder;
        }

        public ParameterVM BuildFor(ParameterTemplate tParam)
        {
            var valueType = tParam.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var paramVM = new ParameterVM()
            {
                IsVisible = true,
                Name = tParam.FriendlyName,
                InternalName = tParam.Name,
                Description = tParam.XmlComments.Summary,
                IsNonReference = tParam.IsNonReference,
                IsObject = !tParam.IsNonReference && !tParam.IsCollection,
                IsCollection = tParam.IsCollection,     
            };

            if (tParam.IsDomainObject)
            {
                paramVM.AllowedClassTypes = _ClassHierarchyBuilder
                                            .GetSubClasses((ClassTemplate)tParam.InnerClass, true, true)
                                            .Select(t => t.FullName)
                                            .ToArray();
            }

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(tParam, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(paramVM, tParam, actualType);

            paramVM.State = _ParameterStateVmBuilder.BuildFor(tParam, tParam.ParameterType.GetDefaultValue());

            return paramVM;
        }

    }
}