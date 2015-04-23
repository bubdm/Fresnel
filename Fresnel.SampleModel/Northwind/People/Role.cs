using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind.People
{
    public abstract class Role
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        [Display(AutoGenerateField = false)]
        public string HiddenProperty { get; set; }

        public override string ToString()
        {
            return typeof(Role).Name;
        }
    }
}