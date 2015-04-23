using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.TestTypes;
using System.Linq;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class ProductFilterQuerySpecification : IQuerySpecification<ObjectWithCtorInjection, Product>
    {
        private IPersistenceService _PersistenceService;

        public ProductFilterQuerySpecification(IPersistenceService persistenceService)
        {
            _PersistenceService = persistenceService;
        }

        public IQueryable<Product> GetResults()
        {
            return this.GetResults(null);
        }

        public IQueryable<Product> GetResults(ObjectWithCtorInjection requestor)
        {
            // If we wanted, we could use the requestor as part of the query clause:

            var results = _PersistenceService.GetObjects<Product>()
                                .Where(p => !p.Name.Contains("Test"));
            return results;
        }
    }
}