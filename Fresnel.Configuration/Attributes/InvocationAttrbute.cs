using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class InvocationAttrbute : Attribute
    {
        public InvocationAttrbute()
        {
        }

        public InvocationAttrbute(string name)
        {
            this.PromptForUnsavedChanges = false;
        }

        public bool PromptForUnsavedChanges { get; set; }
    }
}