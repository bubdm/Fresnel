
using System.Collections.Generic;
using System.Text;

namespace Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used to encapsulate queries that get executed against a repository
    /// </summary>
    public interface IQuerySpecification<TResult>
    {

        /// <summary>
        /// Returns a set of results
        /// </summary>
        /// <returns></returns>
        IEnumerable<TResult> GetResults();

    }
}

