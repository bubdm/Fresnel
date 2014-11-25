using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{
    /// <summary>
    /// Checks if an Assembly is a Framework assembly (e.g. .NET Framework, Fresnel, Nhibernate, etc)
    /// </summary>
    public class IsFrameworkAssemblySpecification : ISpecification<AssemblyName>
    {
        private readonly List<byte[]> _KnownPublicKeyTokens;

        public IsFrameworkAssemblySpecification()
        {
            _KnownPublicKeyTokens = CreateKnownPublicKeyTokenList();
        }

        private List<byte[]> CreateKnownPublicKeyTokenList()
        {
            var results = new List<byte[]>();

            // Add to this list as necessary (use Assembly.GetPublicKeyToken() for the values)

            // .NET Framework:
            results.Add(new byte[] { 183, 122, 92, 86, 25, 52, 224, 137 });
            results.Add(new byte[] { 176, 63, 95, 127, 17, 213, 10, 58 });

            // NHibernate + Castle
            results.Add(new byte[] { 170, 149, 242, 7, 121, 141, 253, 180 });
            results.Add(new byte[] { 64, 125, 208, 128, 141, 68, 251, 220 });

            // Fresnel:
            results.Add(new byte[] { 1, 220, 146, 135, 78, 188, 72, 218 });

            // AzRoles:
            results.Add(new byte[] { 49, 191, 56, 86, 173, 54, 78, 53 });

            // Ionic.Utils.Zip:
            results.Add(new byte[] { 197, 81, 225, 121, 135, 125, 162, 70 });

            // Autofac
            results.Add(new byte[] { 23, 134, 58, 241, 75, 0, 68, 218 });

            return results;
        }

        public IAssertion IsSatisfiedBy(AssemblyName sender)
        {
            var assemblyToken = sender.GetPublicKeyToken();
            var isKnown = _KnownPublicKeyTokens.Any(t => Enumerable.SequenceEqual(assemblyToken, t));

            return isKnown ? Assertion.Pass() :
                             Assertion.FailWithWarning(string.Concat(sender.FullName, " is not a known Framework assembly"));
        }

    }

}
