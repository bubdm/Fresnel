using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Organisation : IParty
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Role> Roles { get; set; }

        public string Name { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address PrimaryAddress { get; set; }

        public string RegistrationNumber { get; set; }

    }

}