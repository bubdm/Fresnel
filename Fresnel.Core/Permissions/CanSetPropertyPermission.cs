using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanSetPropertyPermission : ISpecification<BasePropertyObserver>
    {
        public IAssertion IsSatisfiedBy(BasePropertyObserver oProperty)
        {
            var assertions = new AssertionSet();
            var tProperty = oProperty.Template;

            if (!tProperty.PropertyInfo.CanWrite)
            {
                assertions.AddFailure(tProperty.Name + " cannot be written to");
            }

            if (tProperty.PropertyInfo.GetSetMethod() == null)
            {
                assertions.AddFailure(tProperty.Name + " does not have a setter");
            }

            var propertyAttr = tProperty.Configurations.Get<PropertyConfiguration>();
            if (!propertyAttr.CanWrite)
            {
                assertions.AddFailure(tProperty.Name + " has not been configured for writing");
            }

            return assertions;
        }
    }
}