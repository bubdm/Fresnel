using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.ComponentModel;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// Determines if the given class has an IAudit property
    /// Typically used to method audit information when persisting Domain Objects
    /// </summary>
    public class IsObjectAuditableSpecification : ISpecification<Type>
    {
        private readonly Type _IAuditType = typeof(IAudit);

        public AggregateException IsSatisfiedBy(Type sender)
        {
            foreach (var prop in sender.GetProperties())
            {
                if (prop.PropertyType == _IAuditType)
                {
                    return null;
                }
            }

            var msg = string.Concat(sender.GetType().Name, " does not have a property of type IAudit");
            return new AggregateException(new WarningException(msg));
        }
    }
}