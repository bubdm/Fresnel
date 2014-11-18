using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public void Invoke(object collection, object item)
        {
            var realCollectionType = _RealTypeResolver.GetRealType(collection.GetType());
            var tCollection = (CollectionTemplate)_TemplateCache.GetTemplate(realCollectionType);

            var realItemType = _RealTypeResolver.GetRealType(item.GetType());
            var tItem = (ClassTemplate)_TemplateCache.GetTemplate(realItemType);

            this.Invoke(tCollection, collection, tItem, item);
        }

        /// <summary>
        /// Creates and returns an instance of the Object, using any zero-arg constructor (including internal/protected/private)
        /// </summary>
        public void Invoke(CollectionTemplate tCollection, object collection, ClassTemplate tItem, object item)
        {
            if (tCollection == null)
                throw new ArgumentNullException("tCollection");

            if (collection == null)
                throw new ArgumentNullException("collection");

            if (item == null)
                throw new ArgumentNullException("itemToAdd");

            tCollection.Add(collection, item, tItem.RealObjectType);
        }

    }
}
