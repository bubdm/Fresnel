using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// This object has dependencies automatically injected into it
    /// </summary>
    public class ObjectWithCtorInjection
    {
        public ObjectWithCtorInjection(IFactory<Product> productFactory)
        {
            this.Product = productFactory.Create();
            this.Name = "This name is provided by default";
        }

        public ObjectWithCtorInjection(IFactory<Product> productFactory, string name)
        {
            this.Product = productFactory.Create();
            this.Name = name;
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This property should have an item created by the injected factory
        /// </summary>
        [FilterQuerySpecification(SpecificationType = typeof(ProductFilterQuerySpecification))]
        public virtual Product Product { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}