using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class ProductConsistencyCheck : IConsistencyCheck<Product>
    {
        public AggregateException Check(Product obj)
        {
            var allExceptions = new List<Exception>();

            if (string.IsNullOrEmpty(obj.Name))
            {
                allExceptions.Add(new ValidationException("Name must be provided"));
            }

            if (string.IsNullOrEmpty(obj.Description))
            {
                allExceptions.Add(new ValidationException("Description must be provided"));
            }

            return new AggregateException(allExceptions);
        }
    }
}