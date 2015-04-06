using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel;
using System.Linq;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// Determines if the given class can be tracked by the framework.
    /// Typically used to identify Domain Objects
    /// </summary>
    public class IsObjectTrackableSpecification : ISpecification<Type>
    {
        private readonly Type _GuidType = typeof(Guid);
        private readonly string _IdSearchText = "Id";

        public AggregateException IsSatisfiedBy(Type classType)
        {
            if (classType.IsDerivedFrom<IEntity>() ||
                classType.IsDerivedFrom<IValueObject>())
                return null;

            var properties = classType.GetProperties();

            // Look for a GUID property that starts or ends in "ID".
            // Possible matches are "Id", "ID", "RecordID", "RecordId", etc
            var idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Name.StartsWith(_IdSearchText, StringComparison.OrdinalIgnoreCase));
            if (idProperty == null)
            {
                idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Name.EndsWith(_IdSearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (idProperty != null)
                return null;

            var msg = string.Concat(classType.Name, " is not a Trackable domain object. Consider implementing a DomainTypes interface, or adding a Guid ID property.");
            return new AggregateException(new WarningException(msg));
        }
    }
}