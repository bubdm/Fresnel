using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;


namespace Envivo.Fresnel.Engine.Observers
{

    /// <summary>
    /// Creates and returns Observers for Domain Objects, Collections, Enums, and object Members
    /// </summary>
    
    public class AbstractObserverBuilder
    {
        private TemplateCache _TemplateCache;
        private Func<object, Type, CollectionTemplate, CollectionObserver> _CollectionObserverFactory;
        private Func<object, Type, ClassTemplate, ObjectObserver> _ObjectObserverFactory;
        private Func<object, Type, NonReferenceTemplate, NonReferenceObserver> _NonReferenceObserverFactory;
        private Func<object, Type, EnumTemplate, EnumObserver> _EnumObserverFactory;

        public AbstractObserverBuilder
        (
            TemplateCache templateCache,
            Func<object, Type, CollectionTemplate, CollectionObserver> collectionObserverFactory,
            Func<object, Type, ClassTemplate, ObjectObserver> objectObserverFactory,
            Func<object, Type, NonReferenceTemplate, NonReferenceObserver> nonReferenceObserverFactory,
            Func<object, Type, EnumTemplate, EnumObserver> enumObserverFactory
        )
        {
            _TemplateCache = templateCache;
            _ObjectObserverFactory = objectObserverFactory;
            _CollectionObserverFactory = collectionObserverFactory;
            _NonReferenceObserverFactory = nonReferenceObserverFactory;
            _EnumObserverFactory = enumObserverFactory;
        }

        /// <summary>
        /// Returns a newly created Observer, based on the Type of the given Object
        /// </summary>
        public BaseObjectObserver BuildFor(object obj, Type objectType)
        {
            var template = _TemplateCache.GetTemplate(objectType);
            BaseObjectObserver result = null;

            if (template is CollectionTemplate)
            {
                result = this.CreateCollectionObserver(obj, objectType, (CollectionTemplate)template);
            }
            else if (template is ClassTemplate)
            {
                result = this.CreateObjectObserver(obj, objectType, (ClassTemplate)template);
            }
            else if (template is NonReferenceObserver)
            {
                result = this.CreateNonReferenceObserver(obj, objectType, (NonReferenceTemplate)template);
            }
            else if (template is EnumObserver)
            {
                result = this.CreateEnumObserver(obj, objectType, (EnumTemplate)template);
            }

            result.FinaliseConstruction();

            return result;
        }

        private CollectionObserver CreateCollectionObserver(object collection, Type collectionType, CollectionTemplate tCollection)
        {
            var result = _CollectionObserverFactory(collection, collectionType, tCollection);

            // Ensure the CollectionObserver is aware of it's contents:
            //if (result.RealObject != null)
            //{
            //    result.BindItemsToCollection();
            //}

            return result;
        }

        private ObjectObserver CreateObjectObserver(object obj, Type objectType, ClassTemplate tClass)
        {
            var result = _ObjectObserverFactory(obj, objectType, tClass);
            return result;
        }

        private NonReferenceObserver CreateNonReferenceObserver(object obj, Type objectType, NonReferenceTemplate tNonReference)
        {
            var result = _NonReferenceObserverFactory(obj, objectType, tNonReference);
            return result;
        }

        private EnumObserver CreateEnumObserver(object obj, Type objectType, EnumTemplate tEnum)
        {
            var result = _EnumObserverFactory(obj, objectType, tEnum);
            return result;
        }

    }

}
