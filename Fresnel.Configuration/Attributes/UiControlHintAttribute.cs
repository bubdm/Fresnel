using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class UiControlHintAttribute : Attribute
    {
        public UiControlHintAttribute()
        {
        }

        public UiControlHintAttribute(UiControlType control)
        {
            this.PreferredUiControl = control;
        }

        public UiControlType PreferredUiControl { get; set; }
    }
}