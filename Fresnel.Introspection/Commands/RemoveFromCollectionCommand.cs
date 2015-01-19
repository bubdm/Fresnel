using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class RemoveFromCollectionCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public RemoveFromCollectionCommand
        (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
        }

        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        public bool Invoke(object collection, object item)
        {
            var realCollectionType = _RealTypeResolver.GetRealType(collection);
            var tCollection = (CollectionTemplate)_TemplateCache.GetTemplate(realCollectionType);

            var realItemType = _RealTypeResolver.GetRealType(item);
            var tItem = (ClassTemplate)_TemplateCache.GetTemplate(realItemType);

            var result = this.Invoke(tCollection, collection, tItem, item);
            return result;
        }

        /// <summary>
        /// Removes the given item from the collection property on the given object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tCollectionProperty"></param>
        /// <param name="item"></param>
        public bool Invoke(object obj, PropertyTemplate tCollectionProperty, object item)
        {
            var collection = tCollectionProperty.GetProperty(obj);

            var tCollection = (CollectionTemplate)tCollectionProperty.InnerClass;

            var realItemType = _RealTypeResolver.GetRealType(item);
            var tItem = (ClassTemplate)_TemplateCache.GetTemplate(realItemType);

            var result = this.Invoke(tCollection, collection, tItem, item);
            return result;
        }

        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        public bool Invoke(CollectionTemplate tCollection, object collection, ClassTemplate tItem, object item)
        {
            if (tCollection == null)
                throw new ArgumentNullException("tCollection");

            if (collection == null)
                throw new ArgumentNullException("collection");

            if (item == null)
                throw new ArgumentNullException("item");

            var result = tCollection.Remove(collection, item, tItem.RealType);
            return result;
        }
    }
}