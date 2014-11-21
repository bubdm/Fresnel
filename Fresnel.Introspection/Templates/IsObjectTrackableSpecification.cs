using System;
using System.Linq;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;

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

        public IAssertion IsSatisfiedBy(Type classType)
        {
            if (classType.IsEntity() || classType.IsValueObject())
                return Assertion.Pass();

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
                return Assertion.Pass();

            var msg = string.Concat(classType.Name, " is not a Trackable domain object. Consider implementing a DomainTypes interface, or adding a Guid ID property.");
            return Assertion.FailWithWarning(msg);
        }
    }

}
