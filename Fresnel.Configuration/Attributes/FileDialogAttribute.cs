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

        public DialogAttribute(FileDialogType dialogType)
        {
            this.DialogType = dialogType;
        }

        public FileDialogType DialogType { get; set; }

        public string Filter { get; set; }
    }
}