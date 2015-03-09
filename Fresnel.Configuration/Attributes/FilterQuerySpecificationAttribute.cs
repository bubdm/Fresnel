using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Defines the QuerySpecification used when selecting items for the associated member
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class FilterQuerySpecificationAttribute : Attribute
    {
        public FilterQuerySpecificationAttribute()
        {
        }

        public FilterQuerySpecificationAttribute(Type specificationType)
        {
            this.SpecificationType = specificationType;
        }

        public Type SpecificationType { get; set; }
    }
}