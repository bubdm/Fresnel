using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Contains all attributes for a particular Template. Attributes will be extracted from a ClassConfiguration if provided.
    /// </summary>
    public class AttributesMap
    {
        private IEnumerable<IMissingAttributeBuilder> _AttributeBuilders;
        private Dictionary<Type, AttributeEntry> _AttributeEntries = new Dictionary<Type, AttributeEntry>();

        public AttributesMap
            (
            IEnumerable<IMissingAttributeBuilder> attributeBuilders
            )
        {
            _AttributeBuilders = attributeBuilders;
        }


        /// <summary>
        /// Returns an entry for the attribute that matches the given Attribute type.
        /// If the attribute doesn't exist, a new one is created and returned
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        public TAttribute Get<TAttribute>()
            where TAttribute : Attribute
        {
            var entry = this.GetEntry(typeof(TAttribute), null);
            return (TAttribute)entry.Value;
        }

        public TAttribute Get<TAttribute, TClass>()
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            var classType = typeof(TAttribute);

            var entry = this.GetEntry(typeof(TAttribute), classType);
            return (TAttribute)entry.Value;
        }

        /// <summary>
        /// Returns the entry for the requested Attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public AttributeEntry GetEntry<TAttribute>()
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            var classType = typeof(TAttribute);
            return this.GetEntry(attributeType, classType);
        }
        
        public void Add(Type key, Attribute attribute, bool isConfiguredAtRunTime)
        {
            var wrapper = new AttributeEntry(attribute, isConfiguredAtRunTime);
            _AttributeEntries[key] = wrapper;
        }

        public void AddRange(IEnumerable<Attribute> attributes, bool isConfiguredAtRunTime)
        {
            foreach (var attr in attributes)
            {
                this.Add(attr.GetType(), attr, isConfiguredAtRunTime);
            }
        }

        /// <summary>
        /// Returns an attribute that matches the given Configuration type.
        /// If the configuration doesn't exist, a new one is created and returned
        /// </summary>
        /// <param name="attributeType"></param>
        private AttributeEntry GetEntry(Type attributeType, Type classType)
        {
            // First we'll try to find an exact match:
            var result = _AttributeEntries.TryGetValueOrNull(attributeType);
            if (result != null)
            {
                return result;
            }

            // See if we've got a Builder that can create one:
            var attributeBuilder = _AttributeBuilders
                                        .SingleOrDefault(a => a.GetType()
                                                               .GetGenericArguments()
                                                               .FirstOrDefault() == attributeType);
            if (attributeBuilder != null)
            {
                var allKnownAttributes = _AttributeEntries.Values.Select(e => e.Value).ToArray();
                var defaultAttr = attributeBuilder.BuildFrom(allKnownAttributes, classType);
                this.Add(attributeType, defaultAttr, false);
            }

            //// That didn't work, so find an entry that is a subclass of the requested type:
            //foreach (var existingConfiguration in this.Values)
            //{
            //    if (existingConfiguration.GetType().IsSubclassOf(configurationType))
            //    {
            //        // Found it - We'll method it to the dictionary to make it faster to locate next time:
            //        this.Add(configurationType, existingConfiguration);
            //        return existingConfiguration;
            //    }
            //}

            // We didn't find anything, so create a default Attribute object with default values:
            var defaultCtor = attributeType.GetConstructor(Type.EmptyTypes);
            if (defaultCtor != null)
            {
                var defaultAttr = (Attribute)defaultCtor.Invoke(null);
                this.Add(attributeType, defaultAttr, false);
            }

            return result;
        }

        public void Remove<TAttribute>()
        {
            var attributeType = typeof(TAttribute);
            var config = _AttributeEntries.TryGetValueOrNull(attributeType);
            if (config != null)
            {
                _AttributeEntries.Remove(attributeType);
            }
        }

    }
}