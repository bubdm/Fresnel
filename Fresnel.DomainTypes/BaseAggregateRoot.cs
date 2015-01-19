using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// An Entity that groups closely related objects with complex associations. Aggregates Roots are used to
    /// (1) Provide a access route for Domain Objects within the cluster
    /// (2) Enforce rules for the entire cluster of objects
    /// (3) Provide a suitable locking point for Domain Objects in a mult-user environment
    /// </summary>
    [Serializable]
    public abstract partial class BaseAggregateRoot : BaseDomainObject, IAggregateRoot
    {
    }
}