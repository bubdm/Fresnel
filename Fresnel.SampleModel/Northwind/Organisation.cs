using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
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