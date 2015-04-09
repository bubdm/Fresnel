using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(Order parentOrder)
        {
            this.ParentOrder = parentOrder;
        }

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