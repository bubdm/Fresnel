using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using System;

namespace Envivo.SampleModel.Factories
{
    public class ProductFactory : IFactory<Product>
    {
        public Product Create()
        {
            var newProduct = new Product()
            {
                ID = Guid.NewGuid(),
                Name = "This was created using ProductFactory.Create()",
            };

            return newProduct;
        }

        public Product CreateForSupplier(Supplier supplier)
        {
            var newProduct = this.Create();
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