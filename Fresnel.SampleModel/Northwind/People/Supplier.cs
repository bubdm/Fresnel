using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.SampleModel.Northwind.Places;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public class Supplier : Role
    {
        [Relationship(Type = RelationshipType.Has)]
        public virtual BaseParty Party { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual Address Address { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual Region Region { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual ContactDetails ContactDetails { get; set; }

        public override string ToString()
        {
            return this.Party != null ?
                    this.Party.ToString() :
                    typeof(Supplier).Name;
        }
    }
}