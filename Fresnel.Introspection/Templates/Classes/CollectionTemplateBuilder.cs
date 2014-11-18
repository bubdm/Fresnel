using System;
using System.Collections.Generic;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Assemblies;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class CollectionTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<CollectionTemplate> _CollectionTemplateFactory;
        private CollectionTypeIdentifier _CollectionTypeIdentifier;
        private TemplateCache _TemplateCache;

        public CollectionTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<CollectionTemplate> collectionTemplateFactory,
            CollectionTypeIdentifier collectionTypeIdentifier,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _CollectionTemplateFactory = collectionTemplateFactory;
            _CollectionTypeIdentifier = collectionTypeIdentifier;
            _TemplateCache = templateCache;
        }

        public ClassTemplate BuildFor(Type objectType, AttributesMap collectionAttributes)
        {
            var result = _CollectionTemplateFactory();

            result.RealObjectType = _RealTypeResolver.GetRealType(objectType);
            result.Name = result.RealObjectType.Name;
            result.FriendlyName = result.RealObjectType.Name.CreateFriendlyName();
            result.FullName = result.RealObjectType.FullName;
            result.ElementType = _CollectionTypeIdentifier.DetermineItemType(result.RealObjectType);
            result.InnerClass = (ClassTemplate)_TemplateCache.GetTemplate(result.RealObjectType);
            result.Attributes = collectionAttributes;

            result.FinaliseConstruction();

            return result;
        }

    }
}
