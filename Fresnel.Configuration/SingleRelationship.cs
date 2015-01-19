using System;

namespace Envivo.Fresnel.Configuration
{
    [Serializable()]
    public enum SingleRelationship
    {
        /// <summary>
        /// The property's value is the Parent (or owner) of the current Object.
        /// </summary>

        OwnedBy,

        /// <summary>
        /// The current Object is the parent of the property's value.
        /// The current Object can method the link to the property's value, but cannot destroy the value.
        /// </summary>

        HasA,

        /// <summary>
        /// The current Object is the owner of this property's value.
        /// The current Object can destroy the property's value.
        /// </summary>

        OwnsA,
    }
}