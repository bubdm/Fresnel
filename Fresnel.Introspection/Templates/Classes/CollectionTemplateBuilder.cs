using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class CollectionTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<CollectionTemplate> _CollectionTemplateFactory;
        private CollectionTypeIdentifier _CollectionTypeIdentifier;

        public CollectionTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<CollectionTemplate> collectionTemplateFactory,
            CollectionTypeIdentifier collectionTypeIdentifier
        )
        {
            _RealTypeResolver = realTypeResolver;
            _CollectionTemplateFactory = collectionTemplateFactory;
            _CollectionTypeIdentifier = collectionTypeIdentifier;
        }

        public CollectionTemplate BuildFor(Type objectType, AttributesMap collectionAttributes)
        {
            var result = _CollectionTemplateFactory();

            result.RealObjectType = _RealTypeResolver.GetRealType(objectType);
            result.Name = result.RealObjectType.Name;
            result.FriendlyName = result.RealObjectType.Name.CreateFriendlyName();
            result.FullName = result.RealObjectType.FullName;
            result.ElementType = _CollectionTypeIdentifier.DetermineItemType(result.RealObjectType);
            result.Attributes = collectionAttributes;

            result.FinaliseConstruction();

            return result;
        }

    }
}
