using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Utils;
using System;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class ParameterTemplateBuilder
    {
        private Func<ParameterTemplate> _paramterFactory;
        private IsObjectTrackableSpecification _IsObjectTrackableSpecification;
        private IDomainDependencyResolver _DomainDependencyResolver;

        public ParameterTemplateBuilder
        (
            Func<ParameterTemplate> paramterFactory,
            IsObjectTrackableSpecification isObjectTrackableSpecification,
            IDomainDependencyResolver domainDependencyResolver
        )
        {
            _paramterFactory = paramterFactory;
            _IsObjectTrackableSpecification = isObjectTrackableSpecification;
            _DomainDependencyResolver = domainDependencyResolver;
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

            result.AssemblyReader = tMethod.AssemblyReader;

            result.FinaliseConstruction();

            this.CheckParameterType(result);

            result.IsVisible = !result.IsDomainDependency;

            return result;
        }

        private void CheckParameterType(ParameterTemplate tParameter)
        {
            var paramType = tParameter.ParameterType;
            var exceptions = _IsObjectTrackableSpecification.IsSatisfiedBy(paramType);
            if (exceptions == null)
            {
                tParameter.IsDomainObject = true;
                tParameter.IsValueObject = paramType.IsDerivedFrom<IValueObject>();
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

            var dependency = _DomainDependencyResolver.Resolve(paramType);
            tParameter.IsDomainDependency = (dependency != null);
        }
    }
}