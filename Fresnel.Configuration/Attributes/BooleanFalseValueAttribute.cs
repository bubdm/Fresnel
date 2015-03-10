using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    /// <summary>
    /// This object owns (Composition) the properties content/s
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class BooleanFalseValueAttribute : Attribute
    {
        public BooleanFalseValueAttribute()
        {
            this.Name = "No";
        }

        public BooleanFalseValueAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}