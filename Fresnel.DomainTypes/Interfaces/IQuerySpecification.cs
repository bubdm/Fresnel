using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used to encapsulate queries that get executed against a repository
    /// </summary>
    public interface IQuerySpecification<TResult>
    {
        /// <summary>
        /// Returns a set of results
        /// </summary>
        IQueryable<TResult> GetResults();
    }

    public interface IQuerySpecification<TRequestor, TResult> : IQuerySpecification<TResult>
        where TRequestor : class
    {
        /// <summary>
        /// Returns a set of results
        /// </summary>
        IQueryable<TResult> GetResults(TRequestor requestor);
    }
}
