using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System;
using System.Reflection;

namespace Envivo.Fresnel.Core
{
    public class Engine
    {
        private AssemblyReaderBuilder _AssemblyReaderBuilder;
        private AssemblyReaderMap _AssemblyReaderMap;
        private RealTypeResolver _RealTypeResolver;

        public Engine
            (
            AssemblyReaderBuilder assemblyReaderBuilder,
            AssemblyReaderMap assemblyReaderMap,
            RealTypeResolver realTypeResolver,
            EventTimeLine eventTimeLine
            )
        {
            _AssemblyReaderBuilder = assemblyReaderBuilder;
            _AssemblyReaderMap = assemblyReaderMap;
            _RealTypeResolver = realTypeResolver;
            this.EventTimeLine = eventTimeLine;
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

        public EventTimeLine EventTimeLine { get; private set; }
    }
}