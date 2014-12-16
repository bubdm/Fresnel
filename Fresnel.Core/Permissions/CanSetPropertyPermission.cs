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

            var propertyAttr = tProperty.Attributes.Get<PropertyAttribute>();
            if (!propertyAttr.CanWrite)
            {
                assertions.AddFailure(tProperty.Name + " has not been configured for writing");
            }

            return assertions;
        }
    }
}
