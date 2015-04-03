using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Envivo.Fresnel.SampleModel.Objects
{

    public class PocoFilterQuerySpecification : IQuerySpecification<DependencyAwareObject, PocoObject>
    {
        private IPersistenceService _PersistenceService;

        public PocoFilterQuerySpecification(IPersistenceService persistenceService)
        {
            _PersistenceService = persistenceService;
        }

        public IQueryable<PocoObject> GetResults()
        {
            return this.GetResults(null);
        }

        public IQueryable<PocoObject> GetResults(DependencyAwareObject requestor)
        {
            // If we wanted, we could use the requestor as part of the query clause:

            var results = _PersistenceService.GetObjects<PocoObject>()
                                .Where(p => !p.NormalText.Contains("Test"));
            return results;
        }
    }
}