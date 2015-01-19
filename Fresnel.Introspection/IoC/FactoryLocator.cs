using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.IoC
{
    public class FactoryLocator
    {
        private TemplateCache _TemplateCache;

        private Dictionary<Type, ClassTemplate> _FactoryMap = new Dictionary<Type, ClassTemplate>();

        public FactoryLocator(TemplateCache templateCache)
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

                var tFactory = (ClassTemplate)_TemplateCache.GetTemplate(type);
                _FactoryMap[productType] = tFactory;
            }
        }

        public ClassTemplate GetFactoryFor(Type classType)
        {
            var result = _FactoryMap.TryGetValueOrNull(classType);
            return result;
        }
    }
}