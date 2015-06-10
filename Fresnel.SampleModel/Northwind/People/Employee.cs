using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.SampleModel.Northwind.Places;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public class Employee : Role
    {
        private ICollection<Territory> _Territories = new List<Territory>();
        private ICollection<Note> _Notes = new List<Note>();

        public Person Person { get; set; }

        [Relationship(Type = RelationshipType.Owns)]
        public Address Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime HiredOn { get; set; }

        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Territory> Territories
        {
            get { return _Territories; }
            set { _Territories = value; }
        }

        [Visibility(IsAllowed = false)]
        public void AddToTerritories(Territory territory)
        {
            territory.AddToEmployees(this);
        }

        [Visibility(IsAllowed = false)]
        public void RemoveFromTerritories(Territory territory)
        {
            territory.RemoveFromEmployees(this);
        }

        [Relationship(Type = RelationshipType.Owns)]
        public virtual ICollection<Note> Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }

        public void AddVacationTime(DateTime lastDayAtWork, DateTime firstDayBackAtWork)
        {
            this.Notes.Add(new Note() { Content = "Vacation starts on " + lastDayAtWork.AddDays(1) });
            this.Notes.Add(new Note() { Content = "Vacation ends on " + firstDayBackAtWork.AddDays(-1) });
        }

        public override string ToString()
        {
            return this.Person != null ?
                this.Person.ToString() :
                typeof(Person).Name; ;
        }
    }
}