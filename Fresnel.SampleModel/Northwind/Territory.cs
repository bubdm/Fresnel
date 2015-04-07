using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Territory
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        /// <summary>
        /// The Employees allocated to this Territory
        /// </summary>
        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Employee> Employees { get; set; }

        public void AddToEmployees(Employee employee)
        {
            if (this.Employees.Contains(employee))
                return;

            this.Employees.Add(employee);

            if (!employee.Territories.Contains(this))
            {
                employee.Territories.Add(this);
            }
        }

        public void RemoveFromEmployees(Employee employee)
        {
            if (!this.Employees.Contains(employee))
                return;

            this.Employees.Remove(employee);

            if (!employee.Territories.Contains(this))
            {
                employee.Territories.Add(this);
            }
        }

    }
}