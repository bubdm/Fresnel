using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.IoC
{
    public class QuerySpecificationLocator
    {
        private TemplateCache _TemplateCache;

        private Dictionary<Type, ClassTemplate> _QuerySpecMap = new Dictionary<Type, ClassTemplate>();

        public QuerySpecificationLocator(TemplateCache templateCache)
        {
            _TemplateCache = templateCache;
        }

        public void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                // We're only interested in concrete implementations:
                if (type.IsInterface)
                    continue;

                Type productType;
                if (!type.IsFactory(out productType))
                    continue;

                var tQuerySpec = (ClassTemplate)_TemplateCache.GetTemplate(type);
                _QuerySpecMap[productType] = tQuerySpec;
            }
        }

        public ClassTemplate GetQuerySpecificationFor(Type classType)
        {
            var result = _QuerySpecMap.TryGetValueOrNull(classType);
            return result;
        }
    }
}