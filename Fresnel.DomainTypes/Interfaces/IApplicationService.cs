namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// A set of stateless operations that are exposed to external consumers (e.g. Web Services)
    /// Application Services should not be confused with Domain Services or Infrastructure services
    /// Consider implementing IDependencyAware to access other dependencies.
    /// </summary>
    public interface IApplicationService
    {
    }
}