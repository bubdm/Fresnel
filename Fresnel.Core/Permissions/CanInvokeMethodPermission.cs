using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.Core.Observers;

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
