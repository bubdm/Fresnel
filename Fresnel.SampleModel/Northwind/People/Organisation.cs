using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.SampleModel.Northwind.Places;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public class Organisation : BaseParty
    {
       
        public string Name { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address PrimaryAddress { get; set; }

        public string RegistrationNumber { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}