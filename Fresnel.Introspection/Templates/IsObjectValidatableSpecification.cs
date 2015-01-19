using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// Determines if the given class can be validated using an IsValid method.
    /// </summary>
    public class IsObjectValidatableSpecification : ISpecification<Type>
    {
        private readonly Type _BoolType = typeof(bool);
        private readonly Type _NullableBool = typeof(bool?);

        public IAssertion IsSatisfiedBy(Type sender)
        {
            var isValidMethod = sender.GetMethod("IsValid");
            if (isValidMethod != null && (isValidMethod.ReturnType == _BoolType || isValidMethod.ReturnType == _NullableBool))
                return Assertion.Pass();

            var msg = string.Concat(sender.GetType().Name, " is not a validatable domain object. Consider implementing a DomainTypes interface, or adding an IsValid()::bool method.");
            return Assertion.FailWithWarning(msg);
        }
    }
}