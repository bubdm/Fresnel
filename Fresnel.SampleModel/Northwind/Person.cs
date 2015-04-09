using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Person : IParty
    {
        private ICollection<Role> _Roles = new List<Role>();

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Role> Roles
        {
            get { return _Roles; }
            set { _Roles = value; }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public NameTitles Title { get; set; }

        public override string ToString()
        {
            return string.Concat(this.Title, " ", this.FirstName, " ", this.LastName);
        }
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