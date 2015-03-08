using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    public class DecimalPlacesAttribute : Attribute
    {
        public DecimalPlacesAttribute()
        {
            this.Places = 3;
        }

        public DecimalPlacesAttribute(int places)
        {
            this.Places = places;
        }

        public int Places { get; set; }
    }
}