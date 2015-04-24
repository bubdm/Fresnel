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

        public MethodTemplate BuildFor(ClassTemplate tParent, MethodInfo methodInfo, AttributesMap methodAttributes)
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
            result.Attributes = methodAttributes;

            result.FinaliseConstruction();

            return result;
        }
    }
}