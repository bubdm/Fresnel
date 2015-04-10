using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class MethodTemplateMap : ReadOnlyDictionary<string, MethodTemplate>
    {
        private IDictionary<MethodInfo, MethodTemplate> _tMethods;
        private IEnumerable<MethodTemplate> _VisibleOnly;
        private IEnumerable<MethodTemplate> _tMethodsForLinking;
        private IEnumerable<MethodTemplate> _tMethodsForUnlinking;

        public MethodTemplateMap
        (
            IDictionary<string, MethodTemplate> items,
            IEnumerable<MethodTemplate> linkerMethods,
            IEnumerable<MethodTemplate> unlinkerMethods
        )
            : base(items)
        {
            _tMethods = items.Values.ToDictionary(i => i.MethodInfo);
            _VisibleOnly = items.Values.Where(i => i.IsVisible && !i.IsFrameworkMember).ToArray();
            _tMethodsForLinking = linkerMethods;
            _tMethodsForUnlinking = unlinkerMethods;
        }

        internal MethodTemplate this[MethodInfo methodInfo]
        {
            get { return _tMethods[methodInfo]; }
        }

        /// <summary>
        /// Returns the Methods that are for end user usage
        /// </summary>
        public IEnumerable<MethodTemplate> VisibleOnly
        {
            get { return _VisibleOnly; }
        }

        /// <summary>
        /// Returns the Methods used for Linking/associating objects
        /// </summary>
        internal IEnumerable<MethodTemplate> ForLinking
        {
            get { return _tMethodsForLinking; }
        }

        /// <summary>
        /// Returns the Methods used for Unlinking/disassociating objects
        /// </summary>
        internal IEnumerable<MethodTemplate> ForUnlinking
        {
            get { return _tMethodsForUnlinking; }
        }


        /// <summary>
        /// Returns the method that accepts the given arguments
        /// </summary>
        /// <param name="tArguments"></param>
        public MethodTemplate FindMethodThatAccepts(ClassTemplate[] tArguments)
        {
            foreach (var method in this.Values)
            {
                var methodInfo = method.MethodInfo;
                var methodParams = methodInfo.GetParameters();

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