using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class StaticMethodInfoMapBuilder
    {

        public MethodInfoMap BuildFor(Type realObjectType)
        {
            var results = new Dictionary<string, MethodInfo>();

            var methods = realObjectType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                // Ignore getter/setter methods:
                if (method.IsSpecialName)
                    continue;

                results.Add(method.Name, method);
            }

            return new MethodInfoMap(results);
        }

    }

}
