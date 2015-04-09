using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Customer : Role
    {
        [Relationship(Type = RelationshipType.Has)]
        public IParty Party { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public ContactDetails Contact { get; set; }
    }
}