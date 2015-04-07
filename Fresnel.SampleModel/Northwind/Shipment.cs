using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

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
    }
}