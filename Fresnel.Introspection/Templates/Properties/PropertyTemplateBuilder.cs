using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class PropertyTemplateBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<PropertyTemplate> _PropertyTemplateFactory;
        private IsObjectTrackableSpecification _IsObjectTrackableSpecification;

        public PropertyTemplateBuilder
        (
            RealTypeResolver realTypeResolver,
            Func<PropertyTemplate> propertyTemplateFactory,
            IsObjectTrackableSpecification isObjectTrackableSpecification
        )
        {
            _RealTypeResolver = realTypeResolver;
            _PropertyTemplateFactory = propertyTemplateFactory;
            _IsObjectTrackableSpecification = isObjectTrackableSpecification;
        }

        public PropertyTemplate BuildFor(ClassTemplate tOuterClass, PropertyInfo propertyInfo, AttributesMap configMap)
        {
            var result = _PropertyTemplateFactory();

            result.Attributes = configMap;
            result.OuterClass = tOuterClass;
            result.PropertyInfo = propertyInfo;
            result.PropertyType = propertyInfo.PropertyType;
            result.Name = propertyInfo.Name;
            result.FriendlyName = result.Name.CreateFriendlyName();
            result.FullName = string.Concat(propertyInfo.ReflectedType.Namespace, ".",
                                            propertyInfo.ReflectedType.Name, ".",
                                            propertyInfo.Name);

            this.CheckPropertyType(result);

            var displayAttr = result.Attributes.GetEntry<DisplayAttribute>();

            // If the Property name starts with "Parent", we'll treat it as a parent:
            if (displayAttr == null && result.FriendlyName.StartsWith("Parent "))
            {
                var relationshipAttr = result.Attributes.Get<RelationshipAttribute>();
                relationshipAttr.Type = RelationshipType.OwnedBy;
                result.IsParentRelationship = true;
            }

            result.FinaliseConstruction();

            return result;
        }

        private void CheckPropertyType(PropertyTemplate tProp)
        {
            var propertyType = tProp.PropertyType;

            var allowedOperationsAttr = tProp.Attributes.Get<AllowedOperationsAttribute>();
            var relationshipAttr = tProp.Attributes.Get<RelationshipAttribute>();

            var check = _IsObjectTrackableSpecification.IsSatisfiedBy(propertyType);
            if (check != null &&
                propertyType.IsNonReference())
            {
                tProp.IsNonReference = true;

                if (propertyType.IsNullableType() || propertyType.IsDerivedFrom<string>())
                {
                    tProp.IsNullableType = true;
                }
                return;
            }

            if (propertyType.IsCollection())
            {
                tProp.IsCollection = true;
                tProp.IsReferenceType = true;

                tProp.CanCreate = allowedOperationsAttr.CanCreate;
                tProp.CanAdd = allowedOperationsAttr.CanAdd;
                tProp.CanRemove = allowedOperationsAttr.CanRemove;

                tProp.IsCompositeRelationship = relationshipAttr.Type == RelationshipType.Owns;
                tProp.IsAggregateRelationship = relationshipAttr.Type == RelationshipType.Has;

                // We don't want modifications to the list to affect the parent:
                //TODO collectionPropConfig.UseOptimisticLock = false;

                //tProp.BackingFieldName = attr.BackingFieldName;
                return;
            }

            if (propertyType.IsValueObject())
            {
                tProp.IsValueObject = true;
                tProp.IsReferenceType = true;

                tProp.IsCompositeRelationship = relationshipAttr.Type == RelationshipType.Owns;
            }

            if (propertyType.IsTrackable())
            {
                tProp.IsDomainObject = true;
                tProp.IsReferenceType = true;

                tProp.CanCreate = allowedOperationsAttr.CanCreate;

                tProp.IsCompositeRelationship = relationshipAttr.Type == RelationshipType.Owns;
                tProp.IsAggregateRelationship = relationshipAttr.Type == RelationshipType.Has;
                tProp.IsParentRelationship = relationshipAttr.Type == RelationshipType.OwnedBy;
            }

            // If the Property name starts with "Parent", we'll treat it as a parent:
            var displayAttr = tProp.Attributes.GetEntry<DisplayAttribute>();
            if (displayAttr == null && tProp.FriendlyName.StartsWith("Parent "))
            {
                relationshipAttr.Type = RelationshipType.OwnedBy;
                tProp.IsParentRelationship = true;
            }

        }
    }
}