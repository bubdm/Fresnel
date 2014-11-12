
using System.Collections.Generic;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used by objects that depend on other services hosted by an IoC container
    /// </summary>
    /// <typeparam name="T">The type of the container (e.g. Unity's IContainer)</typeparam>
    public interface IDependencyAware<T>
    {
        /// <summary>
        /// Provides access to an IoC container. Use a factory method to inject the real container.
        /// </summary>
        T DependencyContainer { get; set; }
    }

}
