using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Person : IParty
    {
        public Person()
        {
            this.Roles = new List<Role>();
        }

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Role> Roles { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public NameTitles Title { get; set; }
    }

    public enum NameTitles
    {
        Ms,
        Miss,
        Mrs,
        Mr,
        Dr,
        Master
    }
}