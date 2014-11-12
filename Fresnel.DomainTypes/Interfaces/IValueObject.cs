
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// A object within a Domain that is described by it's characteristics, not identity.
    /// Recommended to be immutable, but technical constraints (e.g. performance or memory constraints) may dictate otherwise
    /// </summary>
    public interface IValueObject : IDomainObject
    {

    }

}
