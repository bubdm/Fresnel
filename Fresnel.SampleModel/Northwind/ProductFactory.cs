using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using System;

namespace Envivo.SampleModel.Factories
{
    public class ProductFactory : IFactory<Product>
    {
        private IPersistenceService _PersistenceService;

        public ProductFactory(IPersistenceService persistenceService)
        {
            _PersistenceService = persistenceService;
        }

        public Product Create()
        {
            var newProduct = _PersistenceService.CreateObject<Product>();
            if (newProduct == null)
                return null;

            newProduct.ID = Guid.NewGuid();
            newProduct.Name = "This was created using ProductFactory.Create()";

            return newProduct;
        }

        public Product Create(Supplier supplier)
        {
            var newProduct = this.Create();
            if (newProduct == null)
                return null;

            newProduct.ID = Guid.NewGuid();
            newProduct.Name = "This was created using ProductFactory.Create(supplier)";
            
            var stockDetail = new StockDetail()
            {
                 Product = newProduct,
                 Supplier = supplier
            };
            newProduct.Stock.Add(stockDetail);

            return newProduct;
        }
    }
}