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
    public class DisplayBooleanAttribute : Attribute
    {
        public DisplayBooleanAttribute()
        {
            this.TrueValue = "Yes";
            this.FalseValue = "No";
        }

        public string TrueValue { get; set; }

        public string FalseValue { get; set; }
    }
}