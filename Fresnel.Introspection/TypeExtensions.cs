using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Envivo.Fresnel.Introspection
{
    public static class TypeExtensions
    {
        private static IsObjectTrackableSpecification s_IsObjectTrackableSpecification = new IsObjectTrackableSpecification();
        private static IsObjectAuditableSpecification s_IsObjectAuditableSpecification = new IsObjectAuditableSpecification();
        private static IsObjectValidatableSpecification s_IsObjectValidatableSpecification = new IsObjectValidatableSpecification();

        static public Type IQuerySpecificationType = typeof(IQuerySpecification<>);
        static public Type IConsistencyCheckType = typeof(IConsistencyCheck<>);

        static public Type IGenericDictionary = typeof(IDictionary<,>);
        static public Type IGenericCollection = typeof(ICollection<>);
        static public Type IGenericEnumerable = typeof(IEnumerable<>);
        
        /// <summary>
        /// Returns TRUE if the given type can be tracked by the framework
        /// </summary>
        /// <param name="realObjectType"></param>
        public static bool IsTrackable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                                   s_IsObjectTrackableSpecification.IsSatisfiedBy(objectType) == null;

            return isValid;
        }

        /// <summary>
        /// Returns TRUE if the given type has an IAudit property
        /// </summary>
        /// <param name="realObjectType"></param>
        public static bool IsAuditable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                           (objectType.GetConstructor(Type.EmptyTypes) != null) &&
                           s_IsObjectAuditableSpecification.IsSatisfiedBy(objectType) == null;

            return isValid;
        }

        /// <summary>
        /// Returns TRUE if the given type has an IsValid():bool method
        /// </summary>
        /// <param name="realObjectType"></param>
        public static bool IsValidatable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                           s_IsObjectValidatableSpecification.IsSatisfiedBy(objectType) == null;

            return isValid;
        }

    }
}