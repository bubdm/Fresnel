using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class PropertyNameExtractor
    {

        /// <summary>
        /// Returns the name of the given Property method
        /// </summary>
        /// <param name="method"></param>

        public string GetPropertyName(MethodInfo method)
        {
            var name = method.Name;

            var isProperty = (method.IsSpecialName && (name.StartsWith("get_") || name.StartsWith("set_")));
            if (isProperty)
            {
                name = name.Remove(0, 4);
            }

            return name;
        }


    }

}
