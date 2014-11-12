
using System.Collections.Generic;
using System.Text;
using System;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// A object within a Domain that is described by it's characteristics, not identity.
    /// Recommended to be immutable, but special cases allow otherwise.
    /// </summary>
    [Serializable]
    public abstract partial class BaseValueObject : BaseDomainObject, IValueObject
    {

        public override bool Equals(object obj)
        {
            throw new MethodAccessException("Value Objects must be compared using property values. Please override the Equals() method and implement the comparison");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
