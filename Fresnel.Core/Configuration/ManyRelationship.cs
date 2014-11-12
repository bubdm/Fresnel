using System;


namespace Envivo.Fresnel.Core.Configuration
{

    [Serializable()]
    public enum ManyRelationship
    {
        /// <summary>
        /// The current Object is related to all of the Domain Objects within the List.
        /// The current Object can remove the links to the Domain Objects within the List, but cannot destroy those Domain Objects.
        /// </summary>
        /// <remarks></remarks>
        HasMany,

        /// <summary>
        /// The current Object owns all of the Domain Objects within the List.
        /// The current Object can delete the Domain Objects within the List.
        /// </summary>
        /// <remarks></remarks>
        OwnsMany,

    }

}
