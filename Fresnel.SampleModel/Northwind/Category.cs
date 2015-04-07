using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Product> Products { get; set; }
    }
}