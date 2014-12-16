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

            var propertyAttr = tProperty.Attributes.Get<PropertyAttribute>();
            if (!propertyAttr.CanRead)
            {
                assertions.AddFailure(tProperty.Name + " has not been configured for reading");
            }

            return assertions;
        }
    }
}
