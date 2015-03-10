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

        public PropertyTemplate BuildFor(ClassTemplate tOuterClass, PropertyInfo propertyInfo, ConfigurationMap configMap)
        {
            var result = _PropertyTemplateFactory();

            result.Configurations = configMap;
            result.OuterClass = tOuterClass;
            result.PropertyInfo = propertyInfo;
            result.PropertyType = propertyInfo.PropertyType;
            result.Name = propertyInfo.Name;
            result.FriendlyName = result.Name.CreateFriendlyName();
            result.FullName = string.Concat(propertyInfo.ReflectedType.Namespace, ".",
                                            propertyInfo.ReflectedType.Name, ".",
                                            propertyInfo.Name);

            this.CheckPropertyType(result);

            var propertyConfig = result.Configurations.Get<PropertyConfiguration>();

            // If the Property name starts with "Parent", we'll treat it as a parent:
            if (!propertyConfig.IsConfiguredAtRunTime && result.FriendlyName.StartsWith("Parent "))
            {
                var objectPropConfig = result.Configurations.Get<ObjectPropertyConfiguration>();
                objectPropConfig.IsParentRelationship = true;
                result.IsParentRelationship = objectPropConfig.IsParentRelationship;
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

                var collectionPropConfig = tProp.Configurations.Get<CollectionPropertyConfiguration>();
                tProp.CanCreate = collectionPropConfig.CanCreate;
                tProp.CanAdd = collectionPropConfig.CanAdd;
                tProp.CanRemove = collectionPropConfig.CanRemove;

                tProp.IsCompositeRelationship = collectionPropConfig.IsCompositeRelationship;
                tProp.IsAggregateRelationship = collectionPropConfig.IsAggregateRelationship;

                // We don't want modifications to the list to affect the parent:
                collectionPropConfig.UseOptimisticLock = false;

                //tProp.BackingFieldName = attr.BackingFieldName;
                return;
            }

            if (propertyType.IsValueObject())
            {
                tProp.IsValueObject = true;
                tProp.IsReferenceType = true;

                var objectPropConfig = tProp.Configurations.Get<ObjectPropertyConfiguration>();
                objectPropConfig.IsCompositeRelationship = true;
                tProp.IsCompositeRelationship = true;
            }

            if (propertyType.IsTrackable())
            {
                tProp.IsDomainObject = true;
                tProp.IsReferenceType = true;

                var objectPropConfig = tProp.Configurations.Get<ObjectPropertyConfiguration>();
                tProp.CanCreate = objectPropConfig.CanCreate;

                tProp.IsCompositeRelationship = objectPropConfig.IsCompositeRelationship;
                tProp.IsAggregateRelationship = objectPropConfig.IsAggregateRelationship;
                tProp.IsParentRelationship = objectPropConfig.IsParentRelationship;
            }

            // If the Property name starts with "Parent", we'll treat it as a parent:
            var propertyConfig = tProp.Configurations.Get<PropertyConfiguration>();
            if (!propertyConfig.IsConfiguredAtRunTime &&
                tProp.FriendlyName.StartsWith("Parent "))
            {
                var objectPropConfig = tProp.Configurations.Get<ObjectPropertyConfiguration>();
                objectPropConfig.IsParentRelationship = true;
                tProp.IsParentRelationship = objectPropConfig.IsParentRelationship;
            }
        }
    }
}