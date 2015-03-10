using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Boolean member
    /// </summary>
    public class CollectionPropertyConfigurationBuilder : IConfigurationBuilder
    {

        public BaseConfiguration BuildFrom(IEnumerable<Attribute> attributes)
        {
            var result = new CollectionPropertyConfiguration();

            var backingField = attributes.OfType<BackingFieldAttribute>().SingleOrDefault();
            if (backingField != null)
            {
                result.BackingFieldName = backingField.Name;
            }

            var canAdd = attributes.OfType<CanAddAttribute>().SingleOrDefault();
            result.CanAdd = canAdd != null;

            var canRemove = attributes.OfType<CanRemoveAttribute>().SingleOrDefault();
            result.CanAdd = canAdd != null;

            return result;
        }

    }
}