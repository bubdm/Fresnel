using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class MethodInfoMapBuilder
    {
        private readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.Instance;

        public MethodInfoMap BuildFor(Type realObjectType)
        {
            var results = new Dictionary<string, MethodInfo>();

            // Ignore getter/setter methods:
            var realMethods = realObjectType
                                .GetMethods(_flags)
                                .Where(m => !m.IsSpecialName);

            foreach (var method in realMethods)
            {
                var uniqueName = this.CreateUniqueName(method, results);
                results.Add(uniqueName, method);
            }

            return new MethodInfoMap(results);
        }

        private string CreateUniqueName(MethodInfo method, Dictionary<string, MethodInfo> existingMethods)
        {
            var newName = method.Name;
            if (existingMethods.ContainsKey(newName))
            {
                var index = 1;
                while (existingMethods.ContainsKey(newName))
                {
                    newName = method.Name + "_" + index.ToString();
                    index += 1;
                }
            }

            return newName;
        }

    }

}
