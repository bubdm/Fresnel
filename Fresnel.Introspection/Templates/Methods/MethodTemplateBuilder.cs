using System;
using System.Linq;
using System.Reflection;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Utils;

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

        public MethodTemplate BuildFor(BaseClassTemplate tParent, MethodInfo methodInfo, AttributesMap methodAttributes)
        {
            var result = _MethodTemplateFactory();

            result.Name = methodInfo.Name;
            result.FriendlyName = result.Name.CreateFriendlyName();
            result.FullName = string.Concat(methodInfo.ReflectedType.Namespace, ".",
                                            methodInfo.ReflectedType.Name, ".",
                                            methodInfo.Name);
            result.Attributes = methodAttributes;

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

            if (result.Parameters.Any())
                result.FriendlyName += "...";

            return result;
        }

    }
}
