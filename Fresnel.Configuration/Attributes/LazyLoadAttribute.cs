using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class LazyLoadAttribute : Attribute
    {
        public LazyLoadAttribute()
        {
            this.IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
    }
}