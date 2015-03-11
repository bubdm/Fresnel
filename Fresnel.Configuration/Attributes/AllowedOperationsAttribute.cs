using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Method)]
    public class AllowedOperationsAttribute : Attribute
    {
        public AllowedOperationsAttribute()
        {
            this.CanCreate = true;
            this.CanRead = true;
            this.CanModify = true;
            this.CanAdd = true;
            this.CanRemove = true;
            this.CanClear = true;
            this.CanInvoke = true;
        }

        public bool CanCreate { get; set; }

        public bool CanRead { get; set; }

        public bool CanModify { get; set; }

        public bool CanAdd { get; set; }

        public bool CanRemove { get; set; }

        public bool CanClear { get; set; }

        public bool CanInvoke { get; set; }

    }
}