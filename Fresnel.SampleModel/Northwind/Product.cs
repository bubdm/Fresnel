using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Product
    {
        public Product()
        {
            this.Categories = new List<Category>();
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

        [Relationship(Type = RelationshipType.Owns)]
        public virtual ICollection<StockDetail> Stock { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Category> Categories { get; set; }

    }
}