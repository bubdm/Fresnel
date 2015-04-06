using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class MoneyConsistencyCheck : IConsistencyCheck<Money>
    {

        public AggregateException Check(Money obj)
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