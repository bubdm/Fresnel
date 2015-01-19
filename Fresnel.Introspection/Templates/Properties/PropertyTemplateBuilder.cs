using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
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

        public PropertyTemplate BuildFor(ClassTemplate tOuterClass, PropertyInfo propertyInfo, AttributesMap propertyAttributes)
        {
            var result = _PropertyTemplateFactory();

            result.Attributes = propertyAttributes;
            result.OuterClass = tOuterClass;
            result.PropertyInfo = propertyInfo;
            result.PropertyType = propertyInfo.PropertyType;
            result.Name = propertyInfo.Name;
            result.FriendlyName = result.Name.CreateFriendlyName();
            result.FullName = string.Concat(propertyInfo.ReflectedType.Namespace, ".",
                                            propertyInfo.ReflectedType.Name, ".",
                                            propertyInfo.Name);

            this.CheckPropertyType(result);

            var propertyAttr = result.Attributes.Get<PropertyAttribute>();

            // If the Property name starts with "Parent", we'll treat it as a parent:
            if (!propertyAttr.IsConfiguredAtRunTime && result.FriendlyName.StartsWith("Parent "))
            {
                var attr = result.Attributes.Get<ObjectPropertyAttribute>();
                attr.Relationship = SingleRelationship.OwnedBy;
                result.IsParentRelationship = true;
            }

            result.FinaliseConstruction();

            return result;
        }

        private void CheckPropertyType(PropertyTemplate tProp)
        {
            var propertyType = tProp.PropertyType;

            var check = _IsObjectTrackableSpecification.IsSatisfiedBy(propertyType);
            if (check.Failed &&
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

                var attr = tProp.Attributes.Get<CollectionPropertyAttribute>();
                tProp.CanCreate = attr.CanCreate;
                tProp.CanAdd = attr.CanAdd;
                tProp.CanRemove = attr.CanRemove;

                tProp.IsCompositeRelationship = (attr.Relationship == ManyRelationship.OwnsMany);
                tProp.IsAggregateRelationship = (attr.Relationship == ManyRelationship.HasMany);

                // We don't want modifications to the list to affect the parent:
                attr.UseOptimisticLock = false;

                //tProp.BackingFieldName = attr.BackingFieldName;
                return;
            }

            if (propertyType.IsValueObject())
            {
                tProp.IsValueObject = true;
                tProp.IsReferenceType = true;

                var attr = tProp.Attributes.Get<ObjectPropertyAttribute>();
                attr.Relationship = SingleRelationship.OwnsA;
                tProp.IsCompositeRelationship = true;
            }

            if (propertyType.IsTrackable())
            {
                tProp.IsDomainObject = true;
                tProp.IsReferenceType = true;

                var attr = tProp.Attributes.Get<ObjectPropertyAttribute>();
                tProp.CanCreate = attr.CanCreate;

                tProp.IsCompositeRelationship = (attr.Relationship == SingleRelationship.OwnsA);
                tProp.IsAggregateRelationship = (attr.Relationship == SingleRelationship.HasA);
                tProp.IsParentRelationship = (attr.Relationship == SingleRelationship.OwnedBy);
            }

            // If the Property name starts with "Parent", we'll treat it as a parent:
            var propertyAttr = tProp.Attributes.Get<PropertyAttribute>();
            if (!propertyAttr.IsConfiguredAtRunTime &&
                tProp.FriendlyName.StartsWith("Parent "))
            {
                var attr = tProp.Attributes.Get<ObjectPropertyAttribute>();
                attr.Relationship = SingleRelationship.OwnedBy;
                tProp.IsParentRelationship = true;
            }
        }
    }
}