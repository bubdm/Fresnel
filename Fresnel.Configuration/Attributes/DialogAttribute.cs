using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DialogAttribute : Attribute
    {
        public DialogAttribute()
        {
        }

        public DialogAttribute(DialogType dialogType)
        {
            this.DialogType = dialogType;
        }

        public DialogType DialogType { get; set; }

        public string Filter { get; set; }
    }
}