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
    public class RepositoryLocator
    {
        private TemplateCache _TemplateCache;

        private Dictionary<Type, ClassTemplate> _RepositoryMap = new Dictionary<Type, ClassTemplate>();

        public RepositoryLocator(TemplateCache templateCache)
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
                if (!type.IsRepository(out productType))
                    continue;

                var tRepository = (ClassTemplate)_TemplateCache.GetTemplate(type);
                _RepositoryMap[productType] = tRepository;
            }
        }

        public ClassTemplate GetRepositoryFor(Type classType)
        {
            var result = _RepositoryMap.TryGetValueOrNull(classType);
            return result;
        }
    }
}
