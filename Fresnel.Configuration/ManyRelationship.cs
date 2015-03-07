using System;

namespace Envivo.Fresnel.Configuration
{
    public enum ManyRelationship
    {
        /// <summary>
        /// The current Object is related to all of the Domain Objects within the List.
        /// The current Object can method the links to the Domain Objects within the List, but cannot destroy those Domain Objects.
        /// </summary>
        HasMany,

        /// <summary>
        /// The current Object owns all of the Domain Objects within the List.
        /// The current Object can delete the Domain Objects within the List.
        /// </summary>
        OwnsMany,
    }
}