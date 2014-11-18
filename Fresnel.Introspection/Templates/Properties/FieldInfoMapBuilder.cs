using System;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class FieldInfoMapBuilder
    {
        private readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private readonly Type _MulticastDelegateType = typeof(MulticastDelegate);

        public FieldInfoMap BuildFor(Type objectType)
        {
            if (objectType.IsInterface)
            {
                return new FieldInfoMap();
            }

            var results = new Dictionary<string, FieldInfo>();
            var eventDelegates = new Dictionary<string, FieldInfo>();

            // To retrieve all of the fields for a type, we need to scan all of the classes in the hierarchy
            // (the BindingFlags don't provide an option to scan the hierarchy).
            var currentType = objectType;
            while (currentType != null)
            {
                foreach (var field in currentType.GetFields(_flags))
                {
                    if (results.Contains(field.Name))
                        continue;

                    if (field.FieldType.IsSubclassOf(_MulticastDelegateType))
                    {
                        // Keep track of Event Delegate types:
                        eventDelegates.Add(field.Name, field);
                    }
                    else
                    {
                        results.Add(field.Name, field);
                    }
                }

                // Move onto the parent class:
                currentType = currentType.BaseType;
            }

            return new FieldInfoMap(results, eventDelegates);
        }
    }

}
