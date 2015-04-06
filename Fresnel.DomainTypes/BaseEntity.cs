using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// An object within a Domain that has unique identity.
    /// </summary>
    public abstract partial class BaseEntity : BaseDomainObject, IEntity
    {
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as BaseDomainObject;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}