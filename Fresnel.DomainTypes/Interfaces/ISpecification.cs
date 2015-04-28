using System;
using System.ComponentModel;
namespace Envivo.Fresnel.DomainTypes.Interfaces
{

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISpecification : IDomainDependency
    { }

    /// <summary>
    /// Encapsulates a test to be made against a Domain Object
    /// </summary>
    public interface ISpecification<T> : ISpecification
        where T : class
    {
        /// <summary>
        /// Returns TRUE if this specification is met by the given Domain Object
        /// </summary>
        /// <param name="obj"></param>
        AggregateException IsSatisfiedBy(T obj);
    }
}