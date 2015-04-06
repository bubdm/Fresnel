using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Permissions
{
    public class CanGetPropertyPermission : ISpecification<PropertyTemplate>
    {
        public AggregateException IsSatisfiedBy(PropertyTemplate tProperty)
        {
            var allExceptions = new List<Exception>();

            if (!tProperty.PropertyInfo.CanRead)
            {
                allExceptions.Add(new ValidationException(tProperty.Name + " cannot be read"));
            }

            if (tProperty.PropertyInfo.GetGetMethod() == null)
            {
                allExceptions.Add(new ValidationException(tProperty.Name + " does not have a getter"));
            }

            var allowedOperations = tProperty.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanRead)
            {
                allExceptions.Add(new ValidationException(tProperty.Name + " has not been configured for reading"));
            }

            return allExceptions.Any() ?
                    new AggregateException(allExceptions) :
                    null;
        }
    }
}