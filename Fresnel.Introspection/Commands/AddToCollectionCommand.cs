using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class AddToCollectionCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public AddToCollectionCommand
        (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
        }

        /// <summary>
        /// Adds the given item to the collection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        public void Invoke(object collection, object item)
        {
            var realCollectionType = _RealTypeResolver.GetRealType(collection.GetType());
            var tCollection = (CollectionTemplate)_TemplateCache.GetTemplate(realCollectionType);

            var realItemType = _RealTypeResolver.GetRealType(item.GetType());
            var tItem = (ClassTemplate)_TemplateCache.GetTemplate(realItemType);

            this.Invoke(tCollection, collection, tItem, item);
        }

        /// <summary>
        /// Adds the given item to the collection property on the given object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tCollectionProperty"></param>
        /// <param name="item"></param>
        public void Invoke(object obj, PropertyTemplate tCollectionProperty, object item)
        {
            var collection = tCollectionProperty.GetProperty(obj);

            var tCollection = (CollectionTemplate)tCollectionProperty.InnerClass;

            var realItemType = _RealTypeResolver.GetRealType(item.GetType());
            var tItem = (ClassTemplate)_TemplateCache.GetTemplate(realItemType);

            this.Invoke(tCollection, collection, tItem, item);
        }

        /// <summary>
        /// Adds the given item to the collection
        /// </summary>
        public void Invoke(CollectionTemplate tCollection, object collection, ClassTemplate tItem, object item)
        {
            if (tCollection == null)
                throw new ArgumentNullException("tCollection");

            if (collection == null)
                throw new ArgumentNullException("collection");

            if (item == null)
                throw new ArgumentNullException("item");

            tCollection.Add(collection, item, tItem.RealObjectType);
        }

    }
}
