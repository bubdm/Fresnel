using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Envivo.Fresnel.SampleModel.Objects
{

    public class PocoFilterQuerySpecification : IQuerySpecification<ObjectWithCtorInjection, PocoObject>
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

        public IQueryable<PocoObject> GetResults(ObjectWithCtorInjection requestor)
        {
            // If we wanted, we could use the requestor as part of the query clause:

            var results = _PersistenceService.GetObjects<PocoObject>()
                                .Where(p => !p.NormalText.Contains("Test"));
            return results;
        }
    }
}