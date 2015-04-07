using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Supplier : Role
    {
        public Supplier()
        {
            this.SuppliedStock = new List<StockDetail>();
        }

        [Relationship(Type = RelationshipType.Has)]
        public virtual IParty Party { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual Address Address { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual Region Region { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual ContactDetails ContactDetails { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<StockDetail> SuppliedStock { get; set; }

    }
}