using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class OrderItem
    {

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.OwnedBy)]
        public virtual Order ParentOrder { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Customer DeliverTo { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Employee PlacedBy { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Shipper ShippedBy { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

    }
}