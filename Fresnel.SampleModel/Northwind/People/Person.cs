using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public class Person : BaseParty
    {

        public NameTitles Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string ToString()
        {
            return string.Concat(this.Title, " ", this.FirstName, " ", this.LastName);
        }
    }

    public enum NameTitles
    {
        Ms,
        Miss,
        Mrs,
        Mr,
        Dr,
        Master
    }
}