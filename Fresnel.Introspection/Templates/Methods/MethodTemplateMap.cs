using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class MethodTemplateMap : ReadOnlyDictionary<string, MethodTemplate>
    {

        private IDictionary<MethodInfo, MethodTemplate> _tMethods;
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
            _tMethodsForLinking = linkerMethods;
            _tMethodsForUnlinking = unlinkerMethods;
        }

        internal MethodTemplate this[MethodInfo methodInfo]
        {
            get { return _tMethods[methodInfo]; }
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

    }

}
