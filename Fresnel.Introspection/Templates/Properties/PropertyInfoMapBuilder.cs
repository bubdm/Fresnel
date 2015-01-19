using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyInfoMapBuilder
    {
        private BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.Instance;

        public PropertyInfoMap BuildFor(Type realObjectType)
        {
            var existingProperties = new Dictionary<string, PropertyInfo>();

            var results = realObjectType.IsInterface ?
                this.CreatePropertiesForInterface(realObjectType, existingProperties) :
                this.CreatePropertiesForClass(realObjectType, existingProperties);

            return results;
        }

        private PropertyInfoMap CreatePropertiesForClass(Type classType, Dictionary<string, PropertyInfo> existingProperties)
        {
            foreach (var prop in classType.GetProperties(_BindingFlags))
            {
                // We're not interested in properties that require parameters
                if (prop.GetIndexParameters().Length > 0)
                    continue;

                this.Add(prop, existingProperties);
            }

            return new PropertyInfoMap(existingProperties);
        }

        /// <summary>
        /// Loads the properties for all interfaces that are inherited in the given interface type
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <remarks>
        /// Interface inheritance isn't implemented in the same was as class inheritance, so we need to do it manually
        /// </remarks>
        private PropertyInfoMap CreatePropertiesForInterface(Type interfaceType, Dictionary<string, PropertyInfo> existingProperties)
        {
            this.CreatePropertiesForClass(interfaceType, existingProperties);

            foreach (var iface in interfaceType.GetInterfaces())
            {
                if (!iface.IsTrackable())
                    continue;

                this.CreatePropertiesForClass(iface, existingProperties);
            }

            return new PropertyInfoMap(existingProperties);
        }

        private void Add(PropertyInfo item, Dictionary<string, PropertyInfo> existingProperties)
        {
            //  What if we already have a property with the same name?
            //   (a) VB.NET supports overloaded properties
            //       We can't handle this yet (how do we render property overloads in the UI?)
            //   (b) A sub-class might be 'hiding' a property from it's super-class
            //       We need to replace the existing property with the 'new' one

            var existingProperty = existingProperties.TryGetValueOrNull(item.Name);
            if (existingProperty == null)
            {
                existingProperties.Add(item.Name, item);
                return;
            }

            if (item.DeclaringType == existingProperty.DeclaringType)
            {
                // TODO: Get rid of the Trace statement, but retain the ability to expose the message:
                // Looks like an overloaded property:
                Trace.TraceWarning("Property {0}.{1} has multiple declarations. Multiple properties (e.g. overloads) are not supported.",
                    item.DeclaringType.Name, item.Name);
                return;
            }

            // Determine if it's a hidden property:
            var isHiddenProperty = item.DeclaringType.IsDerivedFrom(existingProperty.DeclaringType);
            if (isHiddenProperty)
            {
                // Replace it with the 'new' one:
                existingProperties[item.Name] = item;
            }
        }
    }
}