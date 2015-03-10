using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class MethodTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<MethodTemplate> _MethodTemplateFactory;

        public MethodTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<MethodTemplate> methodTemplateFactory
        )
        {
            _RealTypeResolver = realTypeResolver;
            _MethodTemplateFactory = methodTemplateFactory;
        }

        public MethodTemplate BuildFor(ClassTemplate tParent, MethodInfo methodInfo, ConfigurationMap methodAttributes)
        {
            var result = _MethodTemplateFactory();

            result.OuterClass = tParent;
            result.MethodInfo = methodInfo;
            result.MemberInfo = methodInfo;
            result.Name = methodInfo.Name;
            result.FriendlyName = result.Name.CreateFriendlyName();
            result.FullName = string.Concat(methodInfo.ReflectedType.Namespace, ".",
                                            methodInfo.ReflectedType.Name, ".",
                                            methodInfo.Name);
            result.Configurations = methodAttributes;

            result.FinaliseConstruction();

            if (methodInfo.Name.StartsWith("AddTo") && result.Parameters.Count == 1)
            {
                result.IsCollectionAddMethod = true;
                result.IsVisible = false;
            }

            if (methodInfo.Name.StartsWith("RemoveFrom") && result.Parameters.Count == 1)
            {
                result.IsCollectionRemoveMethod = true;
                result.IsVisible = false;
            }

            return result;
        }
    }
}