using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    public class DisplayOrderAttribute : Attribute
    {
        public IEnumerable<string> PropertyNames { get; set; }
    }
}