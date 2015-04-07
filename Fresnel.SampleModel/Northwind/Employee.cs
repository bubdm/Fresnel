using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Employee : Role
    {
        public Employee()
        {
            this.Territories = new List<Territory>();
        }

        public Person Person { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime HiredOn { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Territory> Territories { get; set; }

        public void AddToTerritories(Territory territory)
        {
            territory.AddToEmployees(this);
        }

        public void RemoveFromTerritories(Territory territory)
        {
            territory.RemoveFromEmployees(this);
        }
    }
}