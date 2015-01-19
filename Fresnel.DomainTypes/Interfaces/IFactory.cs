namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// A set of stateless methods for creating complex Domain Objects of the given type.
    /// Consider using Factories when object constructors start getting complicated.
    /// </summary>
    public interface IFactory<T>
        where T : class
    {
        T Create();
    }
}