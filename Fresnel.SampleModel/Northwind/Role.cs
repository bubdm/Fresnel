using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public abstract class Role
    {
        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

    }
}