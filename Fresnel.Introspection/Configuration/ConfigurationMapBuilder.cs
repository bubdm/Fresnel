using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Contains all Attributes for a particular Template. Attributes will be extracted from a ClassConfiguration if provided.
    /// </summary>

    public class ConfigurationMapBuilder
    {
        private Func<ConfigurationMap> _ConfigurationMapFactory;

        public ConfigurationMapBuilder
            (
            Func<ConfigurationMap> configurationMapFactory
            )
        {
            _ConfigurationMapFactory = configurationMapFactory;
        }

        public ConfigurationMap BuildFor(Type classType)
        {
            return this.BuildFor(classType, null);
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Object Type
        /// </summary>
        /// <param name="classType"></param>
        public ConfigurationMap BuildFor(Type classType, IClassConfiguration classConfig)
        {
            var result = _ConfigurationMapFactory();

            var isCustomConfigurationAvailable = false;
            if (classConfig != null)
            {
                var config = classConfig.ObjectInstanceConfiguration;
                if (config != null)
                {
                    result.Add(config.GetType(), config);
                    config.IsConfiguredAtRunTime = true;
                    isCustomConfigurationAvailable = true;
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                result.Attributes = classType.GetCustomAttributes(true).Cast<Attribute>();
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        public ConfigurationMap BuildFor(PropertyInfo propertyInfo, IClassConfiguration outerClassConfig)
        {
            var result = _ConfigurationMapFactory();

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.PropertyConfigurations.Count > 0)
            {
                PropertyConfiguration attribute;
                if (outerClassConfig.PropertyConfigurations.TryGetValue(propertyInfo.Name, out attribute))
                {
                    result.Add(attribute.GetType(), attribute);
                    attribute.IsConfiguredAtRunTime = true;
                    isCustomConfigurationAvailable = true;
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                result.Attributes = GetInheritedAttributesFrom(propertyInfo);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Method
        /// </summary>
        /// <param name="methodInfo"></param>
        public ConfigurationMap BuildFor(MethodInfo methodInfo, IClassConfiguration outerClassConfig)
        {
            var result = _ConfigurationMapFactory();

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.MethodConfigurations.Count > 0)
            {
                var attribute = outerClassConfig.MethodConfigurations.TryGetValueOrNull(methodInfo.Name);
                if (attribute != null)
                {
                    result.Add(attribute.GetType(), attribute);
                    attribute.IsConfiguredAtRunTime = true;
                    isCustomConfigurationAvailable = true;
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                result.Attributes  = GetInheritedAttributesFrom(methodInfo);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Parameter
        /// </summary>
        /// <param name="parameterInfo"></param>
        public ConfigurationMap BuildFor(ParameterInfo parameterInfo, IClassConfiguration outerClassConfig)
        {
            var result = _ConfigurationMapFactory();

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.ParameterConfigurations.Count > 0)
            {
                // CODE SMELL: This is tightly coupled with ClassConfiguration.ConfigureParameter:
                var key = string.Concat(parameterInfo.Member.Name, "%", parameterInfo.Name);
                var attribute = outerClassConfig.ParameterConfigurations.TryGetValueOrNull(key);
                if (attribute != null)
                {
                    result.Add(attribute.GetType(), attribute);
                    attribute.IsConfiguredAtRunTime = true;
                    isCustomConfigurationAvailable = true;
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                result.Attributes = parameterInfo.GetCustomAttributes(true).Cast<Attribute>();
            }

            return result;
        }

        //-----
        private IEnumerable<Attribute> GetInheritedAttributesFrom(PropertyInfo propertyInfo)
        {
            var allTypes = new List<Type>() { propertyInfo.DeclaringType };
            allTypes.AddRange(propertyInfo.DeclaringType.GetSuperClasses());

            // We have to traverse the parent hierarchy manually. See http://stackoverflow.com/a/2520064/80369
            foreach (var type in allTypes)
            {
                var matchingProperty = type.GetProperty(propertyInfo.Name, propertyInfo.PropertyType);
                if (matchingProperty == null)
                    continue;

                var customAttr = matchingProperty.GetCustomAttributes(false).Cast<Attribute>();
                if (customAttr != null)
                {
                    return customAttr;
                }
            }

            return null;
        }

        private IEnumerable<Attribute> GetInheritedAttributesFrom(MethodInfo methodInfo)
        {
            var allTypes = new List<Type>() { methodInfo.DeclaringType };
            allTypes.AddRange(methodInfo.DeclaringType.GetSuperClasses());

            // We have to traverse the parents manualy. See http://stackoverflow.com/a/2520064/80369
            foreach (var type in allTypes)
            {
                var parameterTypes = new List<Type>();
                foreach (var paramInfo in methodInfo.GetParameters())
                {
                    parameterTypes.Add(paramInfo.ParameterType);
                }

                var matchingMethod = type.GetMethod(methodInfo.Name, parameterTypes.ToArray());
                if (matchingMethod == null)
                    continue;

                var customAttr = matchingMethod.GetCustomAttributes(false).Cast<Attribute>();
                if (customAttr != null)
                {
                    return customAttr;
                }
            }

            return null;
        }

    }
}