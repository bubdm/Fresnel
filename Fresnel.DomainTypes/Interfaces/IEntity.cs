
using System.Collections.Generic;
using System.Text;
using System;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Any object within a Domain that has unique identity.
    /// It may be necessary to override Equals() so that comparisons are made by ID.
    /// </summary>
    public interface IEntity : IDomainObject
    {
        /// <summary>
        /// The unique identifier for this Entity
        /// </summary>
        Guid ID { get; set; }

    }
}
