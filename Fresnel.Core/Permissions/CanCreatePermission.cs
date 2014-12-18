using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanCreatePermission : ISpecification<ClassTemplate>
    {

        public IAssertion IsSatisfiedBy(ClassTemplate tClass)
        {
            var assertions = new AssertionSet();

            //if (!tClass.HasDefaultConstructor)
            //{
            //    assertions.AddFailure(tClass.Name + " doesn't have a default constructor");
            //}

            if (tClass.RealType.IsInterface)
            {
                assertions.AddFailure(tClass.Name + " is an interface");
            }

            if (tClass.RealType.IsAbstract)
            {
                assertions.AddFailure(tClass.Name + " is an abstract/base type");
            }

            var classAttr = tClass.Attributes.Get<ObjectInstanceAttribute>();
            if (!classAttr.IsCreatable)
            {
                assertions.AddFailure(tClass.Name + " has not been configured for creation");
            }

            return assertions;
        }
    }
}
