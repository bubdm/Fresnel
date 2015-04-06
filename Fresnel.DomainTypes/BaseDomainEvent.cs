using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// Captures the state of an Entity (or Entities) at a point in time.
    /// Each Domain Event is unique and should NOT be treated as a Value Object.
    /// However, Domain Events should be immutable to avoid the risk of corruption.
    /// </summary>
    public abstract partial class BaseDomainEvent : BaseDomainObject, IDomainEvent
    {
        public virtual DateTime OccurredAt { get; set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            IEntity entity = obj as IEntity;
            if (entity == null)
                return false;

            return this.ID.Equals(entity.ID);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}