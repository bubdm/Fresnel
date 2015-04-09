using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Category
    {
        private ICollection<Product> _Products = new List<Product>();

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
        public virtual ICollection<Product> Products
        {
            get { return _Products; }
            set { _Products = value; }
        }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}