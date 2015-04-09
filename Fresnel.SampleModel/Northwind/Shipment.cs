using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Shipment
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Order Order { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Shipper Shipper { get; set; }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}