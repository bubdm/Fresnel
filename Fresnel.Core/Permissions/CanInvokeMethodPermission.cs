using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using System;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanInvokeMethodPermission : ISpecification<MethodObserver>
    {
        public AggregateException IsSatisfiedBy(MethodObserver oMethod)
        {
            return null; 
        }
    }
}