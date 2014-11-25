using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class NonReferenceTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<NonReferenceTemplate> _NonReferenceTemplateFactory;

        public NonReferenceTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<NonReferenceTemplate> nonReferenceTemplateFactory
        )
        {
            _RealTypeResolver = realTypeResolver;
            _NonReferenceTemplateFactory = nonReferenceTemplateFactory;
        }

        public NonReferenceTemplate BuildFor(Type objectType, AttributesMap attributes)
        {
            var result = _NonReferenceTemplateFactory();

            result.RealObjectType = _RealTypeResolver.GetRealType(objectType);
            result.Name = result.RealObjectType.Name;
            result.FriendlyName = result.RealObjectType.Name.CreateFriendlyName();
            result.FullName = result.RealObjectType.FullName;
            result.Attributes = attributes;

            return result;
        }

    }
}
