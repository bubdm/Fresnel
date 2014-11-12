using System;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Contains all Attributes for a particular Template. Attributes will be extracted from a ClassConfiguration if provided.
    /// </summary>
    /// <remarks></remarks>
    public class AttributesCollection : Dictionary<Type, BaseAttribute>
    {

        /// <summary>
        /// Returns an attribute that matches the given Attribute type.
        /// If the attribute doesn't exist, a new one is created and returned
        /// </summary>
        /// <param name="requestedAttributeType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private BaseAttribute GetAttribute(Type requestedAttributeType)
        {
            // First we'll try to find an exact match:
            var attr = this.TryGetValueOrNull(requestedAttributeType);
            if (attr != null)
            {
                return attr;
            }

            // That didn't work, so find an entry that is a subclass of the requested type:
            foreach (var existingAttr in this.Values)
            {
                if (existingAttr.GetType().IsSubclassOf(requestedAttributeType))
                {
                    // Found it - We'll add it to the dictionary to make it faster to locate next time:
                    this.Add(requestedAttributeType, existingAttr);
                    return existingAttr;
                }
            }

            // We didn't find anything, so create a default Attribute object with default values:
            attr = (BaseAttribute)Activator.CreateInstance(requestedAttributeType);
            this.Add(requestedAttributeType, attr);
            return attr;
        }

        /// <summary>
        /// Returns an attribute that matches the given Attribute type.
        /// If the attribute doesn't exist, a new one is created and returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : BaseAttribute
        {
            return (T)this.GetAttribute(typeof(T));
        }


    }
}
