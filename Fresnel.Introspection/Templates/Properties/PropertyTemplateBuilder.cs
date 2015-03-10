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

            var displayAttr = result.Attributes.Get<DisplayAttribute>();

            // If the Property name starts with "Parent", we'll treat it as a parent:
            if (displayAttr == null && result.FriendlyName.StartsWith("Parent "))
            {
                result.Attributes.Remove<OwnsAttribute>();
                result.Attributes.Remove<HasAttribute>();
                result.Attributes.Add(typeof(OwnedByAttribute), new OwnedByAttribute(), true);
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

                tProp.CanCreate = tProp.Attributes.Get<CanCreateAttribute>() != null;
                tProp.CanAdd = tProp.Attributes.Get<CanAddAttribute>() != null;
                tProp.CanRemove = tProp.Attributes.Get<CanRemoveAttribute>() != null;

                tProp.IsCompositeRelationship = tProp.Attributes.Get<OwnsAttribute>() != null;
                tProp.IsAggregateRelationship = tProp.Attributes.Get<HasAttribute>() != null;

                // We don't want modifications to the list to affect the parent:
                //TODO collectionPropConfig.UseOptimisticLock = false;

                //tProp.BackingFieldName = attr.BackingFieldName;
                return;
            }

            if (propertyType.IsValueObject())
            {
                tProp.IsValueObject = true;
                tProp.IsReferenceType = true;

                tProp.IsCompositeRelationship = tProp.Attributes.Get<OwnsAttribute>() != null;
                tProp.Attributes.Remove<OwnedByAttribute>();
                tProp.Attributes.Remove<HasAttribute>();
            }

            if (propertyType.IsTrackable())
            {
                tProp.IsDomainObject = true;
                tProp.IsReferenceType = true;

                tProp.CanCreate = tProp.Attributes.Get<CanCreateAttribute>() != null;

                tProp.IsCompositeRelationship = tProp.Attributes.Get<OwnsAttribute>() != null;
                tProp.IsAggregateRelationship = tProp.Attributes.Get<HasAttribute>() != null;
                tProp.IsParentRelationship = tProp.Attributes.Get<OwnedByAttribute>() != null;
            }

            // If the Property name starts with "Parent", we'll treat it as a parent:
            var displayAttr = tProp.Attributes.Get<DisplayAttribute>();
            if (displayAttr == null && tProp.FriendlyName.StartsWith("Parent "))
            {
                tProp.Attributes.Remove<OwnsAttribute>();
                tProp.Attributes.Remove<HasAttribute>();
                tProp.Attributes.Add(typeof(OwnedByAttribute), new OwnedByAttribute(), true);
                tProp.IsParentRelationship = true;
            }

        }
    }
}