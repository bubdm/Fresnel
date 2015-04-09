using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class StockDetail
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Product Product { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Supplier Supplier { get; set; }

        public double UnitPrice { get; set; }

        public int QuantityPerUnit { get; set; }

        public int UnitsInStock { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}