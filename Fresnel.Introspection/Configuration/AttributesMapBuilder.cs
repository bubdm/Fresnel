using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    public class AttributesMapBuilder
    {
        private Func<AttributesMap> _AttributesMapFactory;

        public AttributesMapBuilder
            (
            Func<AttributesMap> attributesMapFactory
            )
        {
            _AttributesMapFactory = attributesMapFactory;
        }

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
            var result = _AttributesMapFactory();
            result.TemplateType = classType;

            var isCustomConfigurationAvailable = false;
            if (classConfig != null)
            {
                var attributes = classConfig.ClassAttributes;
                if (attributes != null)
                {
                    isCustomConfigurationAvailable = attributes.Any();
                    result.AddRange(attributes, AttributeSource.ConfigurationClass);
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                var attributes = classType.GetCustomAttributes(true).Cast<Attribute>();
                result.AddRange(attributes, AttributeSource.Decoration);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        public AttributesMap BuildFor(PropertyInfo propertyInfo, IClassConfiguration outerClassConfig)
        {
            var result = _AttributesMapFactory();
            result.TemplateType = propertyInfo.PropertyType;

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.PropertyAttributes.Any())
            {
                var attributes = outerClassConfig.PropertyAttributes.TryGetValueOrNull(propertyInfo.Name);
                if (attributes != null)
                {
                    isCustomConfigurationAvailable = attributes.Any();
                    result.AddRange(attributes, AttributeSource.ConfigurationClass);
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                var attributes = GetInheritedAttributesFrom(propertyInfo);
                result.AddRange(attributes, AttributeSource.Decoration);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Method
        /// </summary>
        /// <param name="methodInfo"></param>
        public AttributesMap BuildFor(MethodInfo methodInfo, IClassConfiguration outerClassConfig)
        {
            var result = _AttributesMapFactory();

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.MethodAttributes.Any())
            {
                var attributes = outerClassConfig.MethodAttributes.TryGetValueOrNull(methodInfo.Name);
                if (attributes != null)
                {
                    isCustomConfigurationAvailable = attributes.Any();
                    result.AddRange(attributes, AttributeSource.ConfigurationClass);
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                var attributes = GetInheritedAttributesFrom(methodInfo);
                result.AddRange(attributes, AttributeSource.Decoration);
            }

            return result;
        }

        /// <summary>
        /// Instantiates and initialises the attributes for the given Parameter
        /// </summary>
        /// <param name="parameterInfo"></param>
        public AttributesMap BuildFor(ParameterInfo parameterInfo, IClassConfiguration outerClassConfig)
        {
            var result = _AttributesMapFactory();
            result.TemplateType = parameterInfo.ParameterType;

            var isCustomConfigurationAvailable = false;

            if (outerClassConfig != null && outerClassConfig.ParameterAttributes.Count > 0)
            {
                // CODE SMELL: This is tightly coupled with ClassConfiguration.ConfigureParameter:
                var key = string.Concat(parameterInfo.Member.Name, "%", parameterInfo.Name);
                var attributes = outerClassConfig.ParameterAttributes.TryGetValueOrNull(key);
                if (attributes != null)
                {
                    isCustomConfigurationAvailable = attributes.Any();
                    result.AddRange(attributes, AttributeSource.ConfigurationClass);
                }
            }

            if (isCustomConfigurationAvailable == false)
            {
                // Use default values:
                var attributes = parameterInfo.GetCustomAttributes(true).Cast<Attribute>();
                result.AddRange(attributes, AttributeSource.Decoration);
            }

            return result;
        }

        //-----
        private IEnumerable<Attribute> GetInheritedAttributesFrom(PropertyInfo propertyInfo)
        {
            var results = new List<Attribute>();

            var allTypes = new List<Type>() { propertyInfo.DeclaringType };
            allTypes.AddRange(propertyInfo.DeclaringType.GetSuperClasses());

            // We have to traverse the parent hierarchy manually. See http://stackoverflow.com/a/2520064/80369
            foreach (var type in allTypes)
            {
                var matchingProperty = type.GetProperty(propertyInfo.Name, propertyInfo.PropertyType);
                if (matchingProperty == null)
                    continue;

                var customAttrs = matchingProperty.GetCustomAttributes(false).Cast<Attribute>();
                if (customAttrs != null)
                {
                    results.AddRange(customAttrs);
                }
            }

            return results;
        }

        private IEnumerable<Attribute> GetInheritedAttributesFrom(MethodInfo methodInfo)
        {
            var results = new List<Attribute>();

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

                var customAttrs = matchingMethod.GetCustomAttributes(false).Cast<Attribute>();
                if (customAttrs != null)
                {
                    results.AddRange(customAttrs);
                }
            }

            return results;
        }

    }
}