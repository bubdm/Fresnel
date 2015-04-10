using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Order
    {
        private ICollection<OrderItem> _OrderItem = new List<OrderItem>();

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Customer DeliverTo { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Employee PlacedBy { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Shipper ShippedBy { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual ICollection<OrderItem> OrderItems
        {
            get { return _OrderItem; }
            set { _OrderItem = value; }
        }

        public override string ToString()
        {
            return string.Concat(typeof(Order).Name, "/", this.OrderNumber);
        }
    }
}