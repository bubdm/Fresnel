using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

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