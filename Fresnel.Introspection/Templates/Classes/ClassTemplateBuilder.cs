using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class ClassTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<ClassTemplate> _ClassTemplateFactory;

        public ClassTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<ClassTemplate> classTemplateFactory
        )
        {
            _RealTypeResolver = realTypeResolver;
            _ClassTemplateFactory = classTemplateFactory;
        }

        public ClassTemplate BuildFor(Type classType, AttributesMap classAttributes)
        {
            var result = _ClassTemplateFactory();

            result.RealType = _RealTypeResolver.GetRealType(classType);
            result.Name = result.RealType.Name;
            result.FriendlyName = result.RealType.Name.CreateFriendlyName();
            result.FullName = result.RealType.FullName;
            result.Attributes = classAttributes;
            
            result.FinaliseConstruction();

            return result;
        }

    }
}
