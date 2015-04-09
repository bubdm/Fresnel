using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Shipper : Role
    {
        [Relationship(Type = RelationshipType.Has)]
        public IParty Party { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address Address { get; set; }
    }
}