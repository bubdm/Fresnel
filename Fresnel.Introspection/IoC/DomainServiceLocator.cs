using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Introspection.IoC
{
    public class DomainServiceLocator
    {
        private TemplateCache _TemplateCache;

        private Dictionary<Type, ClassTemplate> _DomainServiceMap = new Dictionary<Type, ClassTemplate>();

        public DomainServiceLocator(TemplateCache templateCache)
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

                if (!type.IsDomainService())
                    continue;

                var tDomainService = (ClassTemplate)_TemplateCache.GetTemplate(type);
                _DomainServiceMap[type] = tDomainService;
            }
        }

        public ClassTemplate GetDomainServiceFor(Type classType)
        {
            var result = _DomainServiceMap.TryGetValueOrNull(classType);
            return result;
        }
    }
}
