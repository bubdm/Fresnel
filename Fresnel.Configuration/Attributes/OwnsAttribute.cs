using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This object owns the properties content/s (aka Composition). Deleting this object should delete the contents too.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OwnsAttribute : Attribute
    {
        
    }
}