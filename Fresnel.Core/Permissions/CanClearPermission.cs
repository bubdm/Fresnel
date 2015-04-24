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
    public class CanClearPermission : ISpecification<PropertyTemplate>
    {
        public AggregateException IsSatisfiedBy(PropertyTemplate tProperty)
        {
            var allExceptions = new List<Exception>();

            if (!tProperty.IsReferenceType)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " is not a reference type"));
            }

            if (!tProperty.IsNullableType || !tProperty.IsReferenceType)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " is not a nullable type"));
            }

            if (!tProperty.PropertyInfo.CanWrite)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " cannot be written to"));
            }

            if (tProperty.PropertyInfo.GetSetMethod() == null)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " does not have a setter"));
            }

            var allowedOperations = tProperty.Attributes.Get<AllowedOperationsAttribute>();
            if (!allowedOperations.CanClear)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " has not been configured for clearing"));
            }

            if (tProperty.IsParentRelationship)
            {
                allExceptions.Add(new CoreException(tProperty.Name + " is a parent, and cannot be cleared from here"));
            }

            return allExceptions.Any() ?
                    new AggregateException(allExceptions) :
                    null;
        }
    }
}