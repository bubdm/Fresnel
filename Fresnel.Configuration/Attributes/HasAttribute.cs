using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This object is related to (Aggregate relationship) the property's contents
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class HasAttribute : Attribute
    {

    }
}