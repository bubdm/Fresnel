using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;

namespace Envivo.Fresnel.Core
{
    public class Engine
    {
        private AssemblyReaderBuilder _AssemblyReaderBuilder;
        private AssemblyReaderMap _AssemblyReaderMap;

        private Func<UserSession> _UserSessionFactory;

        public Engine
            (
            AssemblyReaderBuilder assemblyReaderBuilder,
            AssemblyReaderMap assemblyReaderMap,
            Func<UserSession> userSessionFactory
            )
        {
            _AssemblyReaderBuilder = assemblyReaderBuilder;
            _AssemblyReaderMap = assemblyReaderMap;
            _UserSessionFactory = userSessionFactory;
        }

        public void RegisterDomainAssembly(Assembly domainAssembly)
        {
            var newReader = _AssemblyReaderBuilder.BuildFor(domainAssembly, false);
            _AssemblyReaderMap.Add(domainAssembly, newReader);
        }

        public void RegisterNonDomainAssembly(Assembly nonDomainAssembly)
        {
            var newReader = _AssemblyReaderBuilder.BuildFor(nonDomainAssembly, true);
            _AssemblyReaderMap.Add(nonDomainAssembly, newReader);
        }

        public UserSession CreateNewSession()
        {
            var result = _UserSessionFactory();
            return result;
        }

    }
}
