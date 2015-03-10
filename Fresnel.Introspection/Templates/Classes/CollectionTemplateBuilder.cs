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

        public CollectionTemplate BuildFor(Type objectType, ConfigurationMap collectionAttributes)
        {
            var result = _CollectionTemplateFactory();

            //result.RealType = _RealTypeResolver.GetRealType(objectType);
            result.RealType = objectType;
            result.Name = result.RealType.Name;
            result.FriendlyName = result.RealType.Name.CreateFriendlyName();
            result.FullName = result.RealType.FullName;
            result.ElementType = _CollectionTypeIdentifier.DetermineItemType(result.RealType);
            result.Configurations = collectionAttributes;

            result.FinaliseConstruction();

            return result;
        }
    }
}