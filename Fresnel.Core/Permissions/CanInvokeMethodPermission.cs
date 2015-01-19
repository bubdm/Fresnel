using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanInvokeMethodPermission : ISpecification<MethodObserver>
    {
        public IAssertion IsSatisfiedBy(MethodObserver oMethod)
        {
            var assertions = new AssertionSet();

            return assertions;
        }
    }
}