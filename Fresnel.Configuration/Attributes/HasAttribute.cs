using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This object is related to the property's content/s (aka Aggregation). Deleting this object will not affect these contents.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HasAttribute : Attribute
    {
        
    }
}