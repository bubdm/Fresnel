using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class BackingFieldIdentifier
    {
        internal const string AutoPropertyFieldSuffix = "k__BackingField";

        /// <summary>
        /// Assigns the backing fields for the properties using pseudo-fuzzy matching
        /// </summary>
        public FieldInfo GetBackingField(PropertyTemplate tProperty)
        {
            var tClass = tProperty.OuterClass;
            if (tClass.RealType.IsInterface)
            {
                // Interfaces don't have fields:
                return null;
            }

            foreach (var field in tClass.Fields.Values)
            {
                var isMatch = IsCustomProperty(tProperty, field);

                if (!isMatch)
                {
                    isMatch = IsAutoProperty(tProperty, field);
                }

                if (!isMatch)
                {
                    isMatch = IsSimilarlyNamed(tProperty, field);
                }

                if (isMatch)
                {
                    if (CheckTypesMatch(tProperty, field))
                    {
                        // Got it:
                        return field;
                    }
                }
            }

            return null;
        }

        private bool IsCustomProperty(PropertyTemplate tProperty, FieldInfo field)
        {
            var attr = tProperty.Attributes.Get<PropertyAttribute>();
            return field.Name.IsSameAs(attr.BackingFieldName);
        }

        private bool IsAutoProperty(PropertyTemplate tProperty, FieldInfo field)
        {
            // Auto-property backing fields have the format "<prop_name>k__BackingField".
            // E.g. the auto-property "StartDate" would use the field "<StartDate>k__BackingField"
            var autoPropFieldName = string.Concat("<", tProperty.Name, ">", AutoPropertyFieldSuffix);
            return field.Name.IsSameAs(autoPropFieldName);
        }

        private bool IsSimilarlyNamed(PropertyTemplate tProperty, FieldInfo field)
        {
            var fuzzyPropName = tProperty.Name.Replace("_", string.Empty);
            var fuzzyFieldName = field.Name.Replace("_", string.Empty);

            // Does the field name almost look like the property name?
            var isMatch = fuzzyFieldName.EndsWith(fuzzyPropName, StringComparison.OrdinalIgnoreCase);

            if (isMatch)
            {
                // Are there only 1 or 2 differences between the names?
                var distance = fuzzyFieldName.CalculateDistanceFrom(fuzzyPropName);
                isMatch = (distance <= 2);
            }

            return isMatch;
        }

        private bool CheckTypesMatch(PropertyTemplate tProperty, FieldInfo field)
        {
            var fieldType = field.FieldType;

            // Do we have a nullable type?
            fieldType = fieldType.IsNullableType() ?
                        fieldType.GetGenericArguments()[0] :
                        fieldType;

            return fieldType.IsDerivedFrom(tProperty.PropertyType);
        }

        private void CheckNonAutoProperty(PropertyTemplate tProperty, FieldInfo field)
        {
            if (field == null &&
                tProperty.PropertyInfo.CanWrite &&
                tProperty.OuterClass.AssemblyReader.IsFrameworkAssembly == false)
            {
                // Hmm.. a Settable Property that doesn't have a Backing Field??
                Trace.TraceWarning(string.Concat("Cannot find the Backing Field for ", tProperty.FullName));
            }
        }

    }

}
