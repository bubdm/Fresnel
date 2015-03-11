using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System.ComponentModel.DataAnnotations;

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

            var allowedOperations = tClass.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanCreate)
            {
                assertions.AddFailure(tClass.Name + " has not been configured for creation");
            }

            return assertions;
        }
    }
}