using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Contains all configurations for a particular Template. Configurations will be extracted from a ClassConfiguration if provided.
    /// </summary>
    public class ConfigurationMap
    {
        private IEnumerable<IConfigurationBuilder> _ConfigurationBuilders;
        private Dictionary<Type, Attribute> _KnownAttributes = new Dictionary<Type, Attribute>();

        public ConfigurationMap
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
        public TAttribute Get<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)this.Get(typeof(TAttribute), null);
        }

        public TAttribute Get<TAttribute, TClass>()
            where TAttribute : Attribute
        {
            return (TAttribute)this.Get(typeof(TAttribute), typeof(TClass));
        }


        /// <summary>
        /// Returns an attribute that matches the given Configuration type.
        /// If the configuration doesn't exist, a new one is created and returned
        /// </summary>
        /// <param name="attributeType"></param>
        private Attribute Get(Type attributeType, Type classType)
        {
            // First we'll try to find an exact match:
            var config = _KnownAttributes.TryGetValueOrNull(attributeType);
            if (config != null)
            {
                return config;
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
            var configBuilder = _ConfigurationBuilders.Single(c => c.GetType().GenericTypeArguments.First() == attributeType);
            config = configBuilder.BuildFrom(_KnownAttributes.Values);

            this.Add(attributeType, config);
            return config;
        }
    }
}