using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanGetPropertyPermission : ISpecification<BasePropertyObserver>
    {
        public IAssertion IsSatisfiedBy(BasePropertyObserver oProperty)
        {
            var assertions = new AssertionSet();
            var tProperty = oProperty.Template;

            if (!tProperty.PropertyInfo.CanRead)
            {
                assertions.AddFailure(tProperty.Name + " cannot be read");
            }

            if (tProperty.PropertyInfo.GetGetMethod() == null)
            {
                assertions.AddFailure(tProperty.Name + " does not have a getter");
            }

            var allowedOperations = tProperty.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanRead)
            {
                assertions.AddFailure(tProperty.Name + " has not been configured for reading");
            }

            return assertions;
        }
    }
}