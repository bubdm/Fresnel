using System;
using System.Collections.Generic;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Assemblies;

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

            result.RealObjectType = _RealTypeResolver.GetRealType(classType);
            result.Name = result.RealObjectType.Name;
            result.FriendlyName = result.RealObjectType.Name.CreateFriendlyName();
            result.FullName = result.RealObjectType.FullName;
            result.Attributes = classAttributes;

            result.FinaliseConstruction();

            return result;
        }

    }
}
