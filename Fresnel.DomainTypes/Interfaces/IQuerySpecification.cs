using System.Collections.Generic;

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

        IEnumerable<TResult> GetResults();
    }
}