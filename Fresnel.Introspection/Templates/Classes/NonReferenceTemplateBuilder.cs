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

        public NonReferenceTemplate BuildFor(Type objectType, ConfigurationMap attributes)
        {
            var result = _NonReferenceTemplateFactory();

            //result.RealType = _RealTypeResolver.GetRealType(objectType);
            result.RealType = objectType;
            result.Name = result.RealType.Name;
            result.FriendlyName = result.RealType.Name.CreateFriendlyName();
            result.FullName = result.RealType.FullName;
            result.Attributes = attributes;

            return result;
        }
    }
}