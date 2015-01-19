using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{
    public class AssemblyReaderMapBuilder
    {
        private readonly AssemblyReaderBuilder _AssemblyReaderBuilder;

        public AssemblyReaderMapBuilder(AssemblyReaderBuilder assemblyReaderBuilder)
        {
            _AssemblyReaderBuilder = assemblyReaderBuilder;
        }

        public AssemblyReaderMap BuildFor(IEnumerable<Assembly> domainAssemblies)
        {
            var result = new AssemblyReaderMap(null);

            //var reader = _AssemblyReaderBuilder.Create(domainAssembly, enableInfrastructureServices);
            return result;
        }
    }
}