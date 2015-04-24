using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.SampleModel.Northwind.People;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.Places
{
    public class Territory
    {
        private ICollection<Employee> _Employees = new List<Employee>();

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// The Employees allocated to this Territory
        /// </summary>
        [Relationship(Type = RelationshipType.Has)]
        public virtual ICollection<Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; }
        }

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