using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;
using Envivo.Fresnel.SampleModel.Northwind.Places;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public class Shipper : Role
    {
        [Relationship(Type = RelationshipType.Has)]
        public BaseParty Party { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address Address { get; set; }

        public override string ToString()
        {
            return typeof(Shipper).Name;
        }
    }
}