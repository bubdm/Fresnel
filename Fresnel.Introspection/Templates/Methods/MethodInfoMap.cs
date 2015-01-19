using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class MethodInfoMap : ReadOnlyDictionary<string, MethodInfo>
    {
        public MethodInfoMap(IDictionary<string, MethodInfo> items)
            : base(items)
        {
        }

        /// <summary>
        /// Returns the method that accepts the given arguments
        /// </summary>
        /// <param name="tArguments"></param>

        public MethodInfo FindMethodThatAccepts(ClassTemplate[] tArguments)
        {
            foreach (var method in this.Values)
            {
                var methodParams = method.GetParameters();

                if (methodParams.Length != tArguments.Length)
                    continue;

                var i = 0;
                foreach (var param in methodParams)
                {
                    if (param.ParameterType.IsAssignableFrom(tArguments[i].RealType))
                    {
                        i++;
                    }
                }

                if (i == tArguments.Length)
                {
                    // Found a match for the given params:
                    return method;
                }
            }

            return null;
        }
    }
}