using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Contains all configurations for a particular Template. Configurations will be extracted from a ClassConfiguration if provided.
    /// </summary>
    public class ConfigurationMap : Dictionary<Type, BaseConfiguration>
    {
        private IEnumerable<IConfigurationBuilder> _ConfigurationBuilders;

        public ConfigurationMap
            (
            IEnumerable<IConfigurationBuilder> configurationBuilders
            )
        {
            _ConfigurationBuilders = configurationBuilders;
            this.Attributes = new Attribute[0];
        }

        public IEnumerable<Attribute> Attributes { get; set; }

        /// <summary>
        /// Returns an attribute that matches the given Configuration type.
        /// If the configuration doesn't exist, a new one is created and returned
        /// </summary>
        /// <param name="configurationType"></param>
        private BaseConfiguration Get(Type configurationType)
        {
            // First we'll try to find an exact match:
            var config = this.TryGetValueOrNull(configurationType);
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
            var configBuilder = _ConfigurationBuilders.Single(c => c.GetType().GenericTypeArguments.First() == configurationType);
            config = configBuilder.BuildFrom(this.Attributes);

            this.Add(configurationType, config);
            return config;
        }

        /// <summary>
        /// Returns an attribute that matches the given Attribute type.
        /// If the attribute doesn't exist, a new one is created and returned
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        public T Get<T>() where T : BaseConfiguration
        {
            return (T)this.Get(typeof(T));
        }
    }
}