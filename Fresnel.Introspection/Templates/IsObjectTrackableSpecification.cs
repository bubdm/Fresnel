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

        public IAssertion IsSatisfiedBy(Type sender)
        {
            if (sender.IsEntity() || sender.IsValueObject())
                return Assertion.Pass();

            // Look for a GUID property that starts or ends in "ID".
            // Possible matches are "Id", "ID", "RecordID", "RecordId", etc
            var idProperty = sender.GetProperties().FirstOrDefault(p => p.PropertyType == _GuidType &&
                                 p.Name.StartsWith(_IdSearchText, StringComparison.OrdinalIgnoreCase));
            if (idProperty == null)
            {
                idProperty = sender.GetProperties().FirstOrDefault(p => p.PropertyType == _GuidType &&
                    p.Name.EndsWith(_IdSearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (idProperty != null)
                return Assertion.Pass();

            var msg = string.Concat(sender.Name, " is not a Trackable domain object. Consider implementing a DomainTypes interface, or adding a Guid ID property.");
            return Assertion.FailWithWarning(msg);
        }
    }

}
