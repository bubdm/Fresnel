using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    /// <summary>
    /// The given property may only be edited once. After saving, it can no longer be modified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WriteOnceAttribute : Attribute
    {
        
    }
}