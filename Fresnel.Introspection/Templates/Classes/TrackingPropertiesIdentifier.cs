using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class TrackingPropertiesIdentifier
    {
        private readonly Type _GuidType = typeof(Guid);
        private readonly Type _IAuditType = typeof(IAudit);

        public PropertyTemplate DetermineIdProperty(ClassTemplate tClass, ObjectInstanceConfiguration objectInstanceAttribute)
        {
            var properties = tClass.Properties.Values;

            // See if the Id property has been explicitly stated:
            var idPropertyName = objectInstanceAttribute.IdPropertyName;
            var idProperty = properties.FirstOrDefault(p => p.PropertyType == _GuidType &&
                                                            p.Name.IsSameAs(idPropertyName, true));

            // Look for a GUID property that starts or ends in "ID".
            // Possible matches are "Id", "ID", "RecordID", "RecordId", etc
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

            var propertyAttr = idProperty.Attributes.Get<PropertyConfiguration>();

            // Users aren't allowed to change PK values:
            propertyAttr.CanWrite = false;

            // PK values must be persisted:
            propertyAttr.CanPersist = true;
        }

        public PropertyTemplate DetermineVersionProperty(ClassTemplate tClass, ObjectInstanceConfiguration objectInstanceAttribute)
        {
            var versionPropertyName = objectInstanceAttribute.VersionPropertyName;
            var versionProperty = tClass.Properties.Values.FirstOrDefault(p => p.Name.IsSameAs(versionPropertyName, true));
            if (versionProperty != null && versionProperty.IsNonReference)
            {
                if (versionProperty.PropertyType == typeof(Int64))
                {
                    return versionProperty;
                }
            }

            return null;
        }

        public PropertyTemplate DetermineAuditProperty(ClassTemplate tClass, ObjectInstanceConfiguration objectInstanceAttribute)
        {
            var result = tClass.Properties.Values.FirstOrDefault(p => p.PropertyType.IsDerivedFrom(_IAuditType));
            return result;
        }
    }
}