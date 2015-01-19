using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class ParameterTemplateBuilder
    {
        private Func<ParameterTemplate> _paramterFactory;
        private IsObjectTrackableSpecification _IsObjectTrackableSpecification;

        public ParameterTemplateBuilder
        (
            Func<ParameterTemplate> paramterFactory,
            IsObjectTrackableSpecification isObjectTrackableSpecification
        )
        {
            _paramterFactory = paramterFactory;
            _IsObjectTrackableSpecification = isObjectTrackableSpecification;
        }

        public TemplateCache TemplateCache { get; set; }

        public ParameterTemplate BuildFor(MethodTemplate tMethod, ParameterInfo paramInfo, AttributesMap parameterAttributes)
        {
            var result = _paramterFactory();

            result.OuterClass = tMethod.OuterClass;
            result.OuterMethod = tMethod;
            result.ParameterInfo = paramInfo;
            result.ParameterType = paramInfo.ParameterType;
            result.Attributes = parameterAttributes;
            result.Name = paramInfo.Name.IsEmpty() ?
                            paramInfo.Position.ToString() :
                            paramInfo.Name;

            result.FriendlyName = paramInfo.Name.CreateFriendlyName();

            result.FinaliseConstruction();

            this.CheckParameterType(result);

            return result;
        }

        private void CheckParameterType(ParameterTemplate tParameter)
        {
            var paramType = tParameter.ParameterType;
            var check = _IsObjectTrackableSpecification.IsSatisfiedBy(paramType);
            if (check.Passed)
            {
                tParameter.IsDomainObject = true;
                tParameter.IsValueObject = paramType.IsValueObject();
            }

            if (paramType.IsCollection())
            {
                tParameter.IsCollection = true;
                tParameter.IsDomainObject = false;
            }

            if (paramType.IsNonReference())
            {
                tParameter.IsNonReference = true;
                tParameter.IsCollection = false;
                tParameter.IsDomainObject = false;

                tParameter.IsNullableType = paramType.IsNullableType() || paramType.IsDerivedFrom<string>();
            }
        }
    }
}