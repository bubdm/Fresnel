using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanSetPropertyPermission : ISpecification<PropertyTemplate>
    {
        public IAssertion IsSatisfiedBy(PropertyTemplate tProperty)
        {
            var assertions = new AssertionSet();
            
            if (!tProperty.PropertyInfo.CanWrite)
            {
                assertions.AddFailure(tProperty.Name + " cannot be written to");
            }

            if (tProperty.PropertyInfo.GetSetMethod() == null)
            {
                assertions.AddFailure(tProperty.Name + " does not have a setter");
            }

            var allowedOperations = tProperty.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanModify)
            {
                assertions.AddFailure(tProperty.Name + " has not been configured for writing");
            }

            return assertions;
        }
    }
}