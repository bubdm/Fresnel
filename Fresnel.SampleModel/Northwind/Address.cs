using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

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
    }
}