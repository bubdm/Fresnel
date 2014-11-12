
using System.Collections.Generic;
using System.Text;
using System;
using Fresnel.DomainTypes.Interfaces;

namespace Fresnel.DomainTypes
{
    /// <summary>
    /// An object within a Domain that has unique identity.
    /// </summary>
    [Serializable]
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
