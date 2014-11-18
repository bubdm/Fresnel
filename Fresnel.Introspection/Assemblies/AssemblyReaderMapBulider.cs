using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    public class AssemblyReaderMapBulider 
    {
        private readonly AssemblyReaderBuilder _AssemblyReaderBuilder;

        public AssemblyReaderMapBulider(AssemblyReaderBuilder assemblyReaderBuilder)
        {
            _AssemblyReaderBuilder = assemblyReaderBuilder;
        }

        public AssemblyReaderMap BuildFor(IEnumerable<Assembly> domainAssemblies)
        {
            var result = new AssemblyReaderMap();

            //var result = _AssemblyReaderBuilder.Create(domainAssembly, enableInfrastructureServices);
            return result;
        }

    }

}
