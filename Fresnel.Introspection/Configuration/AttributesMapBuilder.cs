using System;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Configuration
{

    /// <summary>
    /// Contains all Attributes for a particular Template. Attributes will be extracted from a ClassConfiguration if provided.
    /// </summary>
    
    public class AttributesMapBuilder
    {
        public AttributesMap BuildFor(Type classType)
        {
            return this.BuildFor(classType, null);
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Object Type
        /// </summary>
        /// <param name="classType"></param>
        
        public AttributesMap BuildFor(Type classType, IClassConfiguration classConfig)
        {
            var result = new AttributesMap();

            var isCustomConfigurationAvailable = false;
            if (classConfig != null)
            {
                var attribute = classConfig.ObjectInstanceConfiguration;
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
                var defaultAtributes = classType.GetCustomAttributes(true);
                this.LoadAttributesFrom(defaultAtributes, result);
            }

            return result;
        }


        /// <summary>
        /// Instantiates and initialises the attributes for the given Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        public AttributesMap BuildFor(PropertyInfo propertyInfo, IClassConfiguration outerClassConfig)
        {
            var result = new AttributesMap();

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.PropertyConfigurations.Count > 0)
            {
                PropertyAttribute attribute;
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
                var customAtributes = GetInheritedAttributesFrom(propertyInfo);
                this.LoadAttributesFrom(customAtributes, result);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Method
        /// </summary>
        /// <param name="methodInfo"></param>
        
        public AttributesMap BuildFor(MethodInfo methodInfo, IClassConfiguration outerClassConfig)
        {
            var result = new AttributesMap();

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
                var customAtributes = FindAttributesFor(methodInfo);
                this.LoadAttributesFrom(customAtributes, result);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Parameter
        /// </summary>
        /// <param name="parameterInfo"></param>
        
        public AttributesMap BuildFor(ParameterInfo parameterInfo, IClassConfiguration outerClassConfig)
        {
            var result = new AttributesMap();

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
                var defaultAttributes = parameterInfo.GetCustomAttributes(true);
                this.LoadAttributesFrom(defaultAttributes, result);
            }

            return result;
        }

        //-----
        private object[] GetInheritedAttributesFrom(PropertyInfo propertyInfo)
        {
            var allTypes = new List<Type>() { propertyInfo.DeclaringType };
            allTypes.AddRange(propertyInfo.DeclaringType.GetSuperClasses());

            // We have to traverse the parent hierarchy manually. See http://stackoverflow.com/a/2520064/80369
            foreach (var type in allTypes)
            {
                var matchingProperty = type.GetProperty(propertyInfo.Name, propertyInfo.PropertyType);
                if (matchingProperty == null)
                    continue;

                var customAttr = matchingProperty.GetCustomAttributes(false);
                if (customAttr != null)
                {
                    return customAttr;
                }
            }

            return null;
        }

        private object[] FindAttributesFor(MethodInfo methodInfo)
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

                var customAttr = matchingMethod.GetCustomAttributes(false);
                if (customAttr != null)
                {
                    return customAttr;
                }
            }

            return null;
        }

        private void LoadAttributesFrom(object[] attributes, AttributesMap attributesMap)
        {
            if (attributes == null)
                return;

            foreach (var item in attributes)
            {
                var attr = item as BaseAttribute;
                if (attr == null)
                    continue;

                var key = item.GetType();
                if (attributesMap.Contains(key))
                    continue;

                attributesMap.Add(key, (BaseAttribute)item);
            }
        }

    }
}
