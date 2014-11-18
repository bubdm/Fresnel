using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class TrackingPropertiesIdentifier
    {
        private readonly Type  _IAuditType = typeof(IAudit);

        public PropertyTemplate DetermineIdProperty(ClassTemplate tClass, ObjectInstanceAttribute objectInstanceAttribute)
        {
            var idPropertyName = objectInstanceAttribute.IdPropertyName;
            var idProperty = tClass.Properties.Values.FirstOrDefault(p => p.Name.IsSameAs(idPropertyName, true));
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                return idProperty;
            }

            return null;
        }

        public PropertyTemplate DetermineVersionProperty(ClassTemplate tClass, ObjectInstanceAttribute objectInstanceAttribute)
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

        public PropertyTemplate DetermineAuditProperty(ClassTemplate tClass, ObjectInstanceAttribute objectInstanceAttribute)
        {
            var result = tClass.Properties.Values.FirstOrDefault(p => p.PropertyType.IsDerivedFrom(_IAuditType));
            return result;
        }

    }

}
