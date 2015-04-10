using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Address
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        public string Number { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual Country Country { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        public override string ToString()
        {
            return typeof(Address).Name;
        }
    }
}