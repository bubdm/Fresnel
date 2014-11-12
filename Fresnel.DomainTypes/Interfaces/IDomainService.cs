
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// A set of stateless operations, whose behaviours cannot be contained within any Domain Object.
    /// Domain Services should not be confused with Application/Web Services or Infrastructure services.
    /// Consider implementing IDependencyAware to access other dependencies.
    /// </summary>
    public interface IDomainService
    {

    }

}
