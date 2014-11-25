using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class CollectionTypeIdentifier
    {
        /// <summary>
        /// Determines the Type of Object stored in the collection
        /// </summary>

        public Type DetermineItemType(Type realCollectionType)
        {
            var itemType = realCollectionType.GetElementType();

            if (itemType == null)
            {
                itemType = this.DetermineElementForGenericCollection(realCollectionType);
            }

            if (itemType == null)
            {
                itemType = this.DetermineElementForNonGenericCollection(realCollectionType);
            }

            // If we can't determine the type, use 'Object' as the default:
            // (NB: Base generic types don't return a FullName)
            if (itemType == null || itemType.FullName.IsEmpty())
            {
                itemType = typeof(object);
            }

            return itemType;
        }

        private Type DetermineElementForGenericCollection(Type realCollectionType)
        {
            Type result = null;

            // Scan up the inheritance chain to find a Generic type:
            var baseType = realCollectionType;
            while (baseType != null && !baseType.IsGenericType)
            {
                baseType = baseType.BaseType;
            }

            if (baseType != null && baseType.IsGenericType)
            {
                // It's generic, so we can inspect it:
                var types = baseType.GetGenericArguments();
                switch (types.Length)
                {
                    case 1:
                        // The argument is the Value type
                        result = types[0];
                        if (result.IsGenericParameter && result.GetGenericParameterConstraints().Length > 0)
                        {
                            result = result.GetGenericParameterConstraints()[0];
                        }
                        break;

                    case 2:
                        // The first argument is the Key type, the second is the Value type
                        result = types[1];
                        if (result.IsGenericParameter && result.GetGenericParameterConstraints().Length > 0)
                        {
                            result = result.GetGenericParameterConstraints()[0];
                        }
                        break;
                }
            }

            return result;
        }

        private Type DetermineElementForNonGenericCollection(Type realCollectionType)
        {
            // Look for an indexer property, and see what it's Type is:
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            var properties = realCollectionType.GetProperties(flags);

            // We're not interested in properties that require parameters
            var indexerProperty = properties.SingleOrDefault(p => p.GetIndexParameters().Length != 1);

            if (indexerProperty != null)
            {
                return indexerProperty.PropertyType;
            }
            return null;
        }

    }

}
