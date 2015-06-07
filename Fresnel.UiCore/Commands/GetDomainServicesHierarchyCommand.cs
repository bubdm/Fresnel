using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Model.Classes;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetDomainServicesHierarchyCommand : ICommand
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private DomainServicesBuilder _DomainServicesBuilder;

        public GetDomainServicesHierarchyCommand
            (
            AssemblyReaderMap assemblyReaderMap,
            DomainServicesBuilder domainServicesBuilder
            )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _DomainServicesBuilder = domainServicesBuilder;
        }

        public IEnumerable<Namespace> Invoke()
        {
            var assemblyReader = _AssemblyReaderMap.Values.First(a => !a.IsFrameworkAssembly);
            var results = _DomainServicesBuilder.BuildFor(assemblyReader);
            return results;
        }

        public IEnumerable<Namespace> Invoke(Assembly domainAssembly)
        {
            var assemblyReader = _AssemblyReaderMap.TryGetValueOrNull(domainAssembly);
            if (assemblyReader == null)
                return null;

            var results = _DomainServicesBuilder.BuildFor(assemblyReader);
            return results;
        }
    }
}