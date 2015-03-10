using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Contains all attributes for a particular Template. Attributes will be extracted from a ClassConfiguration if provided.
    /// </summary>
    public class AttributesMap
    {
        private IEnumerable<IConfigurationBuilder> _ConfigurationBuilders;
        private Dictionary<Type, AttributeWrapper> _KnownAttributes = new Dictionary<Type, AttributeWrapper>();

        public AttributesMap
            (
            IEnumerable<IConfigurationBuilder> configurationBuilders
            )
        {
            _ConfigurationBuilders = configurationBuilders;
        }

        /// <summary>
        /// Returns an attribute that matches the given Attribute type.
        /// If the attribute doesn't exist, a new one is created and returned
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        public AttributeWrapper Get<TAttribute>()
            where TAttribute : Attribute
        {
            return this.Get(typeof(TAttribute), null);
        }

        public AttributeWrapper Get<TAttribute, TClass>()
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            var classType = typeof(TAttribute);
            return this.Get(attributeType, classType);
        }

        public void Add(Type key, Attribute attribute, bool isConfiguredAtRunTime)
        {
            var wrapper = new AttributeWrapper(attribute, isConfiguredAtRunTime);
            _KnownAttributes[key] = wrapper;
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
        private AttributeWrapper Get(Type attributeType, Type classType)
        {
            // First we'll try to find an exact match:
            var result = _KnownAttributes.TryGetValueOrNull(attributeType);
            if (result != null)
            {
                return result;
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

            //// We didn't find anything, so create a default Attribute object with default values:
            //var configBuilder = _ConfigurationBuilders.Single(c => c.GetType().GenericTypeArguments.First() == attributeType);
            //result = configBuilder.BuildFrom(_KnownAttributes.Values);

            //this.Add(attributeType, result);
            return result;
        }

        public void Remove<TAttribute>()
        {
            var attributeType = typeof(TAttribute);
            var config = _KnownAttributes.TryGetValueOrNull(attributeType);
            if (config != null)
            {
                _KnownAttributes.Remove(attributeType);
            }
        }

    }
}