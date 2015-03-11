using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class TrackingPropertiesIdentifier
    {
        private readonly Type _GuidType = typeof(Guid);
        private readonly Type _IAuditType = typeof(IAudit);

        public PropertyTemplate DetermineIdProperty(ClassTemplate tClass)
        {
            var properties = tClass.Properties.Values;

            // See if the Id property has been explicitly stated:
            // Find a Guid property that has a [Key] attribute:
            var idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Attributes.GetEntry<KeyAttribute>().WasDeclaredInCode);

            // Look for a GUID property that starts or ends in "ID".
            // Possible matches are "Id", "ID", "RecordID", "RecordId", etc
            var idPropertyName = "ID";
            if (idProperty == null)
            {
                idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Name.StartsWith(idPropertyName, StringComparison.OrdinalIgnoreCase));
            }

            if (idProperty == null)
            {
                idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Name.EndsWith(idPropertyName, StringComparison.OrdinalIgnoreCase));
            }

            this.FixIdPropertyAttributes(idProperty);

            return idProperty;
        }

        private void FixIdPropertyAttributes(PropertyTemplate idProperty)
        {
            if (idProperty == null)
                return;

            // Users aren't allowed to change PK values:
            idProperty.IsVisible = false;

            var allowOperations = idProperty.Attributes.Get<AllowedOperationsAttribute>();
            allowOperations.CanModify = false;

            var persistable = idProperty.Attributes.Get<PersistableAttribute>();
            persistable.IsAllowed = false;
        }

        public PropertyTemplate DetermineVersionProperty(ClassTemplate tClass)
        {
            var properties = tClass.Properties.Values;

            // Find a Guid property that has a [ConcurrencyCheck] attribute:
            var versionProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                                 p.Attributes.GetEntry<ConcurrencyCheckAttribute>() != null);

            var isMatch = versionProperty != null &&
                          versionProperty.PropertyType.IsNonReference() &&
                          (versionProperty.PropertyType == typeof(Int32) ||
                          versionProperty.PropertyType == typeof(Int64));

            if (isMatch)
            {
                return versionProperty;
            }

            return null;
        }

        public PropertyTemplate DetermineAuditProperty(ClassTemplate tClass)
        {
            var result = tClass.Properties.Values.FirstOrDefault(p => p.PropertyType.IsDerivedFrom(_IAuditType));
            return result;
        }
    }
}