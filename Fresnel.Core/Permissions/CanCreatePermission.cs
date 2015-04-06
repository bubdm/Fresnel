using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanCreatePermission : ISpecification<ClassTemplate>
    {
        public AggregateException IsSatisfiedBy(ClassTemplate tClass)
        {
            var allExceptions = new List<Exception>();

            if (tClass.RealType.IsInterface)
            {
                allExceptions.Add(new ValidationException(tClass.Name + " is an interface"));
            }

            if (tClass.RealType.IsAbstract)
            {
                allExceptions.Add(new ValidationException(tClass.Name + " is an abstract/base type"));
            }

            var allowedOperations = tClass.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanCreate)
            {
                allExceptions.Add(new ValidationException(tClass.Name + " has not been configured for creation"));
            }

            return allExceptions.Any() ?
                    new AggregateException(allExceptions) :
                    null;
        }
    }
}