using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Product
    {
        private ICollection<StockDetail> _Stock = new List<StockDetail>();
        private ICollection<Category> _Categories = new List<Category>();

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
        public virtual ICollection<StockDetail> Stock
        {
            get { return _Stock; }
            set { _Stock = value; }
        }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Category> Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

    }
}