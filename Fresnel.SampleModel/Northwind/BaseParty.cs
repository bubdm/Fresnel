using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public abstract class BaseParty
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
    }
}